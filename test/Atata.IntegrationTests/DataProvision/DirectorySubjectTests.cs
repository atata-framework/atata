namespace Atata.IntegrationTests.DataProvision;

public sealed class DirectorySubjectTests
{
    [Test]
    public void Ctor_WithNullAsString() =>
        Assert.Throws<ArgumentNullException>(() =>
            new DirectorySubject((null as string)!));

    [Test]
    public void Ctor_WithNullAsDirectoryInfo() =>
        Assert.Throws<ArgumentNullException>(() =>
            new DirectorySubject((null as DirectoryInfo)!));

    [Test]
    public void Ctor_WithEmptyString() =>
        Assert.Throws<ArgumentException>(() =>
            new DirectorySubject(string.Empty));

    [Test]
    public void Name() =>
        new DirectorySubject(Path.Combine("Parent", "Dir"))
            .Name.Should.Be("Dir");

    [Test]
    public void FullName() =>
        new DirectorySubject(Path.Combine("Parent", "Dir"))
            .FullName.Should.Be(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Parent", "Dir"));

    public static class Exists
    {
        [Test]
        public static void True() =>
            new DirectorySubject(AppDomain.CurrentDomain.BaseDirectory)
                .Exists.Should.BeTrue();

        [Test]
        public static void False() =>
            new DirectorySubject(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MissingDirectory"))
                .Exists.Should.BeFalse();

        [Test]
        public static async Task True_WhenAppearsLater()
        {
            using var directoryFixture = DirectoryFixture.CreateUniqueDirectory();

            Task assertionTask = Task.Run(() =>
                new DirectorySubject(Path.Combine(directoryFixture.DirectoryPath, "test"))
                    .Exists.Should.WithinSeconds(5).BeTrue());

            Task fileCreateTask = Task.Run(async () =>
            {
                await Task.Delay(700);
                directoryFixture.CreateDirectory("test");
            });

            await Task.WhenAll(assertionTask, fileCreateTask);
        }
    }

    public static class Directories
    {
        private static DirectoryFixture s_directoryFixture;

        private static DirectorySubject s_sut;

        [OneTimeSetUp]
        public static void SetUpFixture()
        {
            s_directoryFixture = DirectoryFixture.CreateUniqueDirectory()
                .CreateDirectory("dir1")
                .CreateDirectory(Path.Combine("dir1", "dir1_1"))
                .CreateDirectory(Path.Combine("dir1", "dir1_2"))
                .CreateDirectory(Path.Combine("dir1", "dir1_3"))
                .CreateDirectory("dir2");

            s_sut = new DirectorySubject(s_directoryFixture.DirectoryPath, "sut");
        }

        [OneTimeTearDown]
        public static void TearDownFixture() =>
            s_directoryFixture.Dispose();

        [Test]
        public static void Count() =>
            s_sut.Directories.Count().Should.Be(2);

        [Test]
        public static void Count_ProviderName() =>
            s_sut.Directories.Count().ProviderName.ToResultSubject()
                .Should.Be("sut.Directories.Count()");

        [Test]
        public static void IntIndexer()
        {
            s_sut.Directories[0].Name.Should.StartWith("dir");
            s_sut.Directories[1].Name.Should.StartWith("dir");
            s_sut.Directories[0].Name.Should.Not.Be(s_sut.Directories[1].Name);
        }

        [Test]
        public static void IntIndexer_ProviderName() =>
            s_sut.Directories[0].ProviderName.ToResultSubject()
                .Should.Be("sut.Directories[0]");

        [Test]
        public static void StringIndexer() =>
            s_sut.Directories["dir1"].Should.Exist();

        [Test]
        public static void StringIndexer_OfMissingDirectory() =>
            new DirectorySubject(Guid.NewGuid().ToString()).Directories["missing"].Should.Not.Exist();

        [Test]
        public static void StringIndexer_ProviderName() =>
            s_sut.Directories["dir1"].ProviderName.ToResultSubject()
                .Should.Be("sut.Directories[\"dir1\"]");

        [Test]
        public static void StringIndexer_ForSubDirectories() =>
            s_sut.Directories["dir1"].Directories["dir1_2"].Should.Exist();

        [Test]
        public static void StringIndexer_ForSubDirectories_ProviderName() =>
            s_sut.Directories["dir1"].Directories["dir1_2"].ProviderName.ToResultSubject()
                .Should.Be("sut.Directories[\"dir1\"].Directories[\"dir1_2\"]");

        [Test]
        public static void SubDirectoriesCount() =>
            s_sut.Directories["dir1"].Directories.Count().Should.Be(3);

        [Test]
        public static void SubDirectoriesCount_ProviderName() =>
            s_sut.Directories[0].Directories.Count().ProviderName.ToSubject()
                .Should.Be("sut.Directories[0].Directories.Count()");

        [Test]
        public static void SubDirectories_Owner()
        {
            var directory = s_sut.Directories[0];
            Assert.That(directory.Directories.Count().Should.BeGreaterOrEqual(0), Is.EqualTo(directory));
        }

        [Test]
        public static void Names() =>
            s_sut.Directories["dir1"].Directories.Names
                .Should.BeEquivalent("dir1_1", "dir1_2", "dir1_3");

        [Test]
        public static void Names_ProviderName() =>
            s_sut.Directories["dir1"].Directories.Names.ProviderName.ToResultSubject()
                .Should.Be("sut.Directories[\"dir1\"].Directories.Names");
    }

    public static class Files
    {
        private static DirectoryFixture s_directoryFixture;

        private static DirectorySubject s_sut;

        [OneTimeSetUp]
        public static void SetUpFixture()
        {
            s_directoryFixture = DirectoryFixture.CreateUniqueDirectory()
                .CreateFile("1.txt")
                .CreateFile("2.txt")
                .CreateDirectory("dir1");

            s_sut = new DirectorySubject(s_directoryFixture.DirectoryPath, "sut");
        }

        [OneTimeTearDown]
        public static void TearDownFixture() =>
            s_directoryFixture.Dispose();

        [Test]
        public static void Count() =>
            s_sut.Files.Count().Should.Be(2);

        [Test]
        public static void Count_ProviderName() =>
            s_sut.Files.Count().ProviderName.ToResultSubject()
                .Should.Be("sut.Files.Count()");

        [Test]
        public static void Owner() =>
            Assert.That(s_sut.Files.Count().Should.Be(2), Is.EqualTo(s_sut));

        [Test]
        public static void SubDirectories_Owner()
        {
            var directory = s_sut.Directories[0];
            Assert.That(directory.Files.Count().Should.Be(0), Is.EqualTo(directory));
        }

        [Test]
        public static void IntIndexer()
        {
            s_sut.Files[0].Name.Should.EndWith(".txt");
            s_sut.Files[1].Name.Should.EndWith(".txt");
            s_sut.Files[0].Name.Should.Not.Be(s_sut.Files[1].Name);
        }

        [Test]
        public static void StringIndexer() =>
            s_sut.Files["1.txt"].Should.Exist();

        [Test]
        public static void StringIndexer_ProviderName() =>
            s_sut.Files["1.txt"].ProviderName.ToResultSubject()
                .Should.Be("sut.Files[\"1.txt\"]");

        [Test]
        public static void Where_First() =>
            s_sut.Files.Where(x => x.Extension != ".ext").First()
                .Name.Should.EndWith(".txt");

        [Test]
        public static void Where_First_ProviderName() =>
            s_sut.Files.Where(x => x.Extension != ".ext").First()
                .ProviderName.ToResultSubject().Should.Be("sut.Files.Where(x => x.Extension != \".ext\").First()");

        [Test]
        public static void Names() =>
            s_sut.Files.Names
                .Should.BeEquivalent("1.txt", "2.txt");

        [Test]
        public static void Names_ProviderName() =>
            s_sut.Files.Names.ProviderName.ToResultSubject()
                .Should.Be("sut.Files.Names");

        [Test]
        public static void ThroughMissingSubDirectory() =>
            s_sut.Directories["missing"].Files["missing.txt"].Should.Not.Exist();
    }
}
