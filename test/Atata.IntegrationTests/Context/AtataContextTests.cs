using Atata.IntegrationTests.DataProvision;

namespace Atata.IntegrationTests.Context;

public class AtataContextTests : UITestFixture
{
    protected override bool ReuseDriver => false;

    [Test]
    public void RestartDriver()
    {
        AtataContext.Current.RestartDriver();

        Go.To<BasicControlsPage>();
        Assert.That(AtataContext.Current.Driver.Title, Is.Not.Null.Or.Empty);

        AtataContext.Current.RestartDriver();

        Assert.That(AtataContext.Current.Driver.Title, Is.Null.Or.Empty);
        Go.To<BasicControlsPage>();
    }

    public class Artifacts : UITestFixture
    {
        [Test]
        public void SubDirectory_Should_Not_Exist() =>
            AtataContext.Current.Artifacts.Directories["dir1"]
                .Should.Not.Exist();

        [Test]
        public void SubDirectory_ProviderName() =>
            AtataContext.Current.Artifacts.Directories["dir1"].ProviderName.ToResultSubject()
                .Should.Equal("Artifacts.Directories[\"dir1\"]");

        [Test]
        public void FileInSubDirectory_ProviderName() =>
            AtataContext.Current.Artifacts.Directories["dir1"].Files["file.txt"].ProviderName.ToResultSubject()
                .Should.Equal("Artifacts.Directories[\"dir1\"].Files[\"file.txt\"]");

        [Test]
        public void FileInSubDirectory_Should_Not_Exist() =>
            AtataContext.Current.Artifacts.Directories["dir1"].Files["file.txt"].Should.Not.Exist();

        [Test]
        public void SubDirectory_Should_ContainFiles()
        {
            var directoryFixture = DirectoryFixture.CreateUniqueDirectoryIn(AtataContext.Current.Artifacts.FullName)
                .CreateFiles("1.txt", "2.txt");

            using (directoryFixture)
                AtataContext.Current.Artifacts.Directories[directoryFixture.DirectoryName]
                    .Should.ContainFiles("1.txt", "2.txt");
        }

        [Test]
        public void SubDirectory_Should_ContainDirectories()
        {
            var directoryFixture = DirectoryFixture.CreateUniqueDirectoryIn(AtataContext.Current.Artifacts.FullName)
                .CreateDirectories("dir1", "dir2");

            using (directoryFixture)
                AtataContext.Current.Artifacts.Directories[directoryFixture.DirectoryName]
                    .Should.ContainDirectories("dir1", "dir2");
        }
    }

    public class Variables : UITestFixtureBase
    {
        [Test]
        public void AddViaBuilder()
        {
            var context = ConfigureBaseAtataContext()
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
                .AddVariable("key1", "val1")
                .Build();

            context.Variables.ToSutSubject()
                .ValueOf(x => x["key1"]).Should.Be("val1");
        }

        [Test]
        public void AddViaContext()
        {
            var context = ConfigureBaseAtataContext()
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
                .Build();

            context.Variables["key1"] = "val1";

            context.Variables.ToSutSubject()
                .ValueOf(x => x["key1"]).Should.Be("val1");
        }
    }

    public class FillTemplateString : UITestFixtureBase
    {
        private Subject<AtataContext> _sut;

        [SetUp]
        public void SetUp() =>
            _sut = ConfigureBaseAtataContext()
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
                .AddVariable("key1", "val1")
                .Build()
                .ToSutSubject();

        [Test]
        public void WithPredefinedVariable() =>
            _sut.ResultOf(x => x.FillTemplateString("start_{test-name}_end"))
                .Should.Be($"start_{nameof(WithPredefinedVariable)}_end");

        [Test]
        public void WithCustomVariable() =>
            _sut.ResultOf(x => x.FillTemplateString("start_{key1}_end"))
                .Should.Be("start_val1_end");

        [Test]
        public void WithMissingVariable() =>
            _sut.ResultOf(x => x.FillTemplateString("start_{missingkey}_end"))
                .Should.Throw<FormatException>();
    }
}
