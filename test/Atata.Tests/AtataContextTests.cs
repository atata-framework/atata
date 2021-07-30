using Atata.Tests.DataProvision;
using NUnit.Framework;

namespace Atata.Tests
{
    public class AtataContextTests : UITestFixture
    {
        protected override bool ReuseDriver => false;

        [Test]
        public void AtataContext_RestartDriver()
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
    }
}
