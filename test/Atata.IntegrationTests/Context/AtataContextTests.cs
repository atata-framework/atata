using System.Text;
using Atata.IntegrationTests.DataProvision;

namespace Atata.IntegrationTests.Context;

public static class AtataContextTests
{
    public sealed class Artifacts : WebDriverSessionTestSuite
    {
        [Test]
        public void SubDirectory_Should_Not_Exist() =>
            CurrentContext.Artifacts.Directories["dir1"]
                .Should.Not.Exist();

        [Test]
        public void SubDirectory_ProviderName() =>
            CurrentContext.Artifacts.Directories["dir1"].ProviderName.ToResultSubject()
                .Should.Be("Artifacts.Directories[\"dir1\"]");

        [Test]
        public void FileInSubDirectory_ProviderName() =>
            CurrentContext.Artifacts.Directories["dir1"].Files["file.txt"].ProviderName.ToResultSubject()
                .Should.Be("Artifacts.Directories[\"dir1\"].Files[\"file.txt\"]");

        [Test]
        public void FileInSubDirectory_Should_Not_Exist() =>
            CurrentContext.Artifacts.Directories["dir1"].Files["file.txt"].Should.Not.Exist();

        [Test]
        public void SubDirectory_Should_ContainFiles()
        {
            var directoryFixture = DirectoryFixture.CreateUniqueDirectoryIn(CurrentContext.ArtifactsPath)
                .CreateFiles("1.txt", "2.txt");

            using (directoryFixture)
                CurrentContext.Artifacts.Directories[directoryFixture.DirectoryName]
                    .Should.ContainFiles("1.txt", "2.txt");
        }

        [Test]
        public void SubDirectory_Should_ContainDirectories()
        {
            var directoryFixture = DirectoryFixture.CreateUniqueDirectoryIn(CurrentContext.ArtifactsPath)
                .CreateDirectories("dir1", "dir2");

            using (directoryFixture)
                CurrentContext.Artifacts.Directories[directoryFixture.DirectoryName]
                    .Should.ContainDirectories("dir1", "dir2");
        }
    }

    public sealed class AddArtifact : SessionlessTestSuite
    {
        private Subject<AtataContext> _sut;

        protected override void OnSetUp() =>
            _sut = CurrentContext.ToSutSubject();

        [Test]
        public void WithNullAsRelativeFilePath() =>
            _sut.Invoking(x => x.AddArtifact(null!, "...", default))
                .Should.ThrowExactly<ArgumentNullException>();

        [Test]
        public void WithNullAsFileContent() =>
            _sut.Invoking(x => x.AddArtifact("a.txt", (null as string)!, default))
                .Should.ThrowExactly<ArgumentNullException>();

        [Test]
        public void WithFileContent() =>
            _sut.Act(x => x.AddArtifact("b.txt", "123"), "AddArtifact(...)")
                .Object.Artifacts.Files["b.txt"].Should.Exist()
                    .ReadAllText().Should.Be("123");

        [Test]
        public void WithFileBytes() =>
            _sut.Act(x => x.AddArtifact("c.txt", Encoding.UTF8.GetBytes("abc")), "AddArtifact(...)")
                .Object.Artifacts.Files["c.txt"].Should.Exist()
                    .ReadAllText().Should.Be("abc");

        [Test]
        public void WithStream()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("xyz"));

            _sut.Act(x => x.AddArtifact("d.txt", stream), "AddArtifact(\"d.txt\", stream)")
                .Object.Artifacts.Files["d.txt"].Should.Exist()
                    .ReadAllText().Should.Be("xyz");
        }

        [Test]
        public void WithFileContentWithExtension() =>
            _sut.Act(x => x.AddArtifact("e", FileContentWithExtension.CreateFromText("qwe", ".txt")), "AddArtifact(...)")
                .Object.Artifacts.Files["e.txt"].Should.Exist()
                    .ReadAllText().Should.Be("qwe");

        [TestCase("a/b.txt")]
        [TestCase("a/b/c.txt")]
        public void WithRelativeFilePath(string relativeFilePath) =>
            _sut.Act(x => x.AddArtifact(relativeFilePath, "123"), "AddArtifact(...)")
                .Object.Artifacts.Files[relativeFilePath].Should.Exist()
                    .ReadAllText().Should.Be("123");

        [Test]
        public void Publishes_ArtifactAddedEvent()
        {
            var handlerActionMock = new Mock<Action<ArtifactAddedEvent>>();
            _sut.Object.EventBus.Subscribe(handlerActionMock.Object);

            _sut.Act(x => x.AddArtifact("f/g.txt", "123", new() { ArtifactType = "art type", ArtifactTitle = "art title" }), "AddArtifact(...)");

            handlerActionMock.Verify(
                action => action(It.Is<ArtifactAddedEvent>(ev =>
                    ev.RelativeFilePath == "f/g.txt" &&
                    ev.AbsoluteFilePath == Path.Combine(_sut.Object.ArtifactsPath, "f/g.txt") &&
                    ev.ArtifactType == "art type" &&
                    ev.ArtifactTitle == "art title")),
                Times.Once);
        }
    }

    public sealed class Variables : TestSuiteBase
    {
        [Test]
        public void SetViaBuilder()
        {
            var context = ConfigureSessionlessAtataContext()
                .UseVariable("key1", "val1")
                .Build();

            context.Variables.ToSutSubject()
                .ValueOf(x => x["key1"]).Should.Be("val1");
        }

        [Test]
        public void SetViaContext()
        {
            var context = ConfigureSessionlessAtataContext()
                .Build();

            context.Variables["key1"] = "val1";

            context.Variables.ToSutSubject()
                .ValueOf(x => x["key1"]).Should.Be("val1");
        }

        public sealed class FillTemplateString : TestSuiteBase
        {
            private Subject<AtataContext> _sut;

            [SetUp]
            public void SetUp() =>
                _sut = ConfigureSessionlessAtataContext()
                    .UseVariable("key1", "val1")
                    .Build()
                    .ToSutSubject();

            [Test]
            public void WithPredefinedVariable() =>
                _sut.ResultOf(x => x.Variables.FillTemplateString("start_{test-name}_end"))
                    .Should.Be($"start_{nameof(WithPredefinedVariable)}_end");

            [Test]
            public void WithCustomVariable() =>
                _sut.ResultOf(x => x.Variables.FillTemplateString("start_{key1}_end"))
                    .Should.Be("start_val1_end");

            [Test]
            public void WithMissingVariable() =>
                _sut.ResultOf(x => x.Variables.FillTemplateString("start_{missingkey}_end"))
                    .Should.Throw<FormatException>();
        }
    }

    public sealed class State : TestSuiteBase
    {
        [Test]
        public void SetViaBuilder()
        {
            var context = ConfigureSessionlessAtataContext()
                .UseState("key1", "val1")
                .Build();

            context.State.ToSutSubject()
                .ValueOf(x => x["key1"]).Should.Be("val1");
        }

        [Test]
        public void SetViaContext()
        {
            var context = ConfigureSessionlessAtataContext()
                .Build();

            context.State["key1"] = "val1";

            context.State.ToSutSubject()
                .ValueOf(x => x["key1"]).Should.Be("val1");
        }

        [Test]
        public void SetWithAutoKeyViaBuilder()
        {
            var context = ConfigureSessionlessAtataContext()
                .UseState("val1")
                .Build();

            context.State.ToSutSubject()
                .ValueOf(x => x.Get<string>()).Should.Be("val1");
        }

        [Test]
        public void SetWithAutoKeyViaContext()
        {
            var context = ConfigureSessionlessAtataContext()
                .Build();

            context.State.Set("val1");

            context.State.ToSutSubject()
                .ValueOf(x => x.Get<string>()).Should.Be("val1");
        }

        [Test]
        public void SetNullWithAutoKeyViaBuilder()
        {
            var context = ConfigureSessionlessAtataContext()
                .UseState<string?>(null)
                .Build();

            context.State.ToSutSubject()
                .ValueOf(x => x.Get<string>()).Should.BeNull();
        }

        [Test]
        public void SetNullWithAutoKeyViaContext()
        {
            var context = ConfigureSessionlessAtataContext()
                .Build();

            context.State.Set<string?>(null);

            context.State.ToSutSubject()
                .ValueOf(x => x.Get<string>()).Should.BeNull();
        }
    }

    public sealed class RaiseAssertionError : SessionlessTestSuite
    {
        private Subject<AtataContext> _sut;

        protected override void OnSetUp() =>
            _sut = CurrentContext.ToSutSubject();

        [Test]
        public void WithMessageAndException()
        {
            InvalidOperationException exception = new("Something went wrong.");

            _sut.Invoking(x => x.RaiseAssertionError("Simulated error.", exception))
                .Should.Throw<AssertionException>()
                    .ValueOf(x => x.Message).Should.Be("Wrong Simulated error.")
                    .ValueOf(x => x.InnerException).Should.Be(exception);

            CurrentLog.GetSnapshotOfLevel(LogLevel.Error).ToSubject("ErrorLogs")
                .Should.ContainSingle()
                .SubjectOf(x => x.Single()).AggregateAssert(x => x
                    .ValueOf(x => x.Message).Should.StartWith("Wrong Simulated error.")
                    .ValueOf(x => x.Message).Should.Contain(exception.Message)
                    .ValueOf(x => x.Exception).Should.BeNull());
        }

        [Test]
        public void WithMessageOnly()
        {
            _sut.Invoking(x => x.RaiseAssertionError("Simulated error.", null))
                .Should.Throw<AssertionException>()
                    .ValueOf(x => x.Message).Should.Be("Wrong Simulated error.")
                    .ValueOf(x => x.InnerException).Should.BeNull();

            CurrentLog.GetSnapshotOfLevel(LogLevel.Error).ToSubject("ErrorLogs")
                .Should.ContainSingle()
                .SubjectOf(x => x.Single()).AggregateAssert(x => x
                    .ValueOf(x => x.Message).Should.StartWith("Wrong Simulated error.")
                    .ValueOf(x => x.Exception).Should.BeNull());
        }
    }
}
