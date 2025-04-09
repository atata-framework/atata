namespace Atata.IntegrationTests.DataProvision;

public sealed class FileSubjectTests
{
    [Test]
    public void Ctor_WithNullAsString() =>
        Assert.Throws<ArgumentNullException>(() =>
            new FileSubject((null as string)!));

    [Test]
    public void Ctor_WithNullAsFileInfo() =>
        Assert.Throws<ArgumentNullException>(() =>
            new FileSubject((null as FileInfo)!));

    [Test]
    public void Ctor_WithEmptyString() =>
        Assert.Throws<ArgumentException>(() =>
            new FileSubject(string.Empty));

    [Test]
    public void Name() =>
        new FileSubject(Path.Combine("Dir", "Some.txt"))
            .Name.Should.Be("Some.txt");

    [Test]
    public void FullName() =>
        new FileSubject(Path.Combine("Dir", "Some.txt"))
            .FullName.Should.Be(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dir", "Some.txt"));

    [Test]
    public void Extension() =>
        new FileSubject(Path.Combine("Dir", "Some.txt"))
            .Extension.Should.Be(".txt");

    [Test]
    public void NameWithoutExtension() =>
        new FileSubject(Path.Combine("Dir", "Some.txt"))
            .NameWithoutExtension.Should.Be("Some");

    [Test]
    public void ProviderName()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SomeDir", "SomeFile.txt");

        new FileSubject(filePath).ProviderName.ToResultSubject()
            .Should.Be($"\"{filePath}\" file");
    }

    public sealed class Exists
    {
        [Test]
        public void True() =>
            new FileSubject(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)[0])
                .Exists.Should.BeTrue();

        [Test]
        public void False() =>
            new FileSubject("MissingFile.txt")
                .Exists.Should.BeFalse();

        [Test]
        public void False_InMissingDirectory() =>
            new FileSubject(Path.Combine("MissingDir", "MissingFile.txt"))
                .Exists.Should.BeFalse();

        [Test]
        public async Task True_WhenAppearsLater()
        {
            using var directoryFixture = DirectoryFixture.CreateUniqueDirectory();

            Task assertionTask = Task.Run(() =>
                new FileSubject(Path.Combine(directoryFixture.DirectoryPath, "test.txt"))
                    .Exists.Should.WithinSeconds(5).BeTrue());

            Task fileCreateTask = Task.Run(async () =>
            {
                await Task.Delay(700);
                directoryFixture.CreateFile("test.txt");
            });

            await Task.WhenAll(assertionTask, fileCreateTask);
        }
    }

    public sealed class ReadAllText
    {
        [Test]
        public void ProviderName() =>
            new FileSubject("some.txt", "sut")
                .ReadAllText().ProviderName.ToResultSubject().Should.Be("sut.ReadAllText() => result");

        [Test]
        public void Executes()
        {
            using var directoryFixture = DirectoryFixture.CreateUniqueDirectory()
                .CreateFile("1.txt", "some text");

            new FileSubject(Path.Combine(directoryFixture.DirectoryPath, "1.txt"), "sut")
                .ReadAllText().Should.Be("some text");
        }

        [Test]
        public void Throws_WhenFileNotFound()
        {
            var text = new FileSubject("missing.txt", "sut")
                .ReadAllText();

            Assert.Throws<FileNotFoundException>(() =>
                _ = text.Object);
        }
    }
}
