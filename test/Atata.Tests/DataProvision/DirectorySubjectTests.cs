using System;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests.DataProvision
{
    [TestFixture]
    public class DirectorySubjectTests
    {
        [Test]
        public void Name() =>
            new DirectorySubject(Path.Combine("Parent", "Dir"))
                .Name.Should.Equal("Dir");

        [Test]
        public void FullName() =>
            new DirectorySubject(Path.Combine("Parent", "Dir"))
                .FullName.Should.Equal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Parent", "Dir"));

        [TestFixture]
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
        }

        [TestFixture]
        public static class Directories
        {
            private static DirectoryFixture directoryFixture;

            private static DirectorySubject sut;

            [OneTimeSetUp]
            public static void SetUpFixture()
            {
                directoryFixture = DirectoryFixture.CreateUniqueDirectory()
                    .CreateDirectory("dir1")
                    .CreateDirectory(Path.Combine("dir1", "dir1_1"))
                    .CreateDirectory(Path.Combine("dir1", "dir1_2"))
                    .CreateDirectory(Path.Combine("dir1", "dir1_3"))
                    .CreateDirectory("dir2");

                sut = new DirectorySubject(directoryFixture.DirectoryPath, "sut");
            }

            [OneTimeTearDown]
            public static void TearDownFxture() =>
                directoryFixture.Dispose();

            [Test]
            public static void Count() =>
                sut.Directories.Count().Should.Equal(2);

            [Test]
            public static void Count_ProviderName() =>
                sut.Directories.Count().ProviderName.ToResultSubject()
                    .Should.Equal("sut.Directories.Count()");

            [Test]
            public static void IntIndexer() =>
                sut.Directories[0].Name.Should.Equal("dir1");

            [Test]
            public static void StringIndexer() =>
                sut.Directories["dir1"].Exists.Should.BeTrue();

            [Test]
            public static void StringIndexer_ProviderName() =>
                sut.Directories["dir1"].ProviderName.ToResultSubject()
                    .Should.Equal("sut.Directories[\"dir1\"]");

            [Test]
            public static void StringIndexer_ForSubDirectories() =>
                sut.Directories["dir1"].Directories["dir1_2"].Exists.Should.BeTrue();

            [Test]
            public static void StringIndexer_ForSubDirectories_ProviderName() =>
                sut.Directories["dir1"].Directories["dir1_2"].ProviderName.ToResultSubject()
                    .Should.Equal("sut.Directories[\"dir1\"].Directories[\"dir1_2\"]");

            [Test]
            public static void SubDirectoriesCount() =>
                sut.Directories[0].Directories.Count().Should.Equal(3);

            [Test]
            public static void SubDirectoriesCount_ProviderName() =>
                sut.Directories[0].Directories.Count().ProviderName.ToSubject()
                    .Should.Equal("sut.Directories[0].Directories.Count()");
        }

        [TestFixture]
        public static class Files
        {
            private static DirectoryFixture directoryFixture;

            private static DirectorySubject sut;

            [OneTimeSetUp]
            public static void SetUpFixture()
            {
                directoryFixture = DirectoryFixture.CreateUniqueDirectory()
                    .CreateFile("1.txt")
                    .CreateFile("2.txt");

                sut = new DirectorySubject(directoryFixture.DirectoryPath, "sut");
            }

            [OneTimeTearDown]
            public static void TearDownFxture() =>
                directoryFixture.Dispose();

            [Test]
            public static void Count() =>
                sut.Files.Count().Should.Equal(2);

            [Test]
            public static void Count_ProviderName() =>
                sut.Files.Count().ProviderName.ToResultSubject()
                    .Should.Equal("sut.Files.Count()");

            [Test]
            public static void IntIndexer() =>
                sut.Files[0].Name.Should.Equal("1.txt");

            [Test]
            public static void StringIndexer() =>
                sut.Files["1.txt"].Exists.Should.BeTrue();

            [Test]
            public static void StringIndexer_ProviderName() =>
                sut.Files["1.txt"].ProviderName.ToResultSubject()
                    .Should.Equal("sut.Files[\"1.txt\"]");

            [Test]
            public static void Where_First() =>
                sut.Files.Where(x => x.Extension != ".ext").First()
                    .Name.Should.Equal("1.txt");

            [Test]
            public static void Where_First_ProviderName() =>
                sut.Files.Where(x => x.Extension != ".ext").First()
                    .ProviderName.ToResultSubject().Should.Equal("sut.Files.Where(x => x.Extension != \".ext\").First()");
        }
    }
}
