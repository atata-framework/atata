using System;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests.DataProvision
{
    [TestFixture]
    public class FileSubjectTests
    {
        [Test]
        public void Ctor_WithNullAsString_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() =>
                new FileSubject(null as string));

        [Test]
        public void Ctor_WithNullAsFileInfo_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() =>
                new FileSubject(null as FileInfo));

        [Test]
        public void Ctor_WithEmptyString_ThrowsArgumentException() =>
            Assert.Throws<ArgumentException>(() =>
                new FileSubject(string.Empty));

        [Test]
        public void Name() =>
            new FileSubject(Path.Combine("Dir", "Some.txt"))
                .Name.Should.Equal("Some.txt");

        [Test]
        public void FullName() =>
            new FileSubject(Path.Combine("Dir", "Some.txt"))
                .FullName.Should.Equal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dir", "Some.txt"));

        [Test]
        public void Extension() =>
            new FileSubject(Path.Combine("Dir", "Some.txt"))
                .Extension.Should.Equal(".txt");

        [Test]
        public void NameWithoutExtension() =>
            new FileSubject(Path.Combine("Dir", "Some.txt"))
                .NameWithoutExtension.Should.Equal("Some");

        [TestFixture]
        public class Exists
        {
            [Test]
            public void True() =>
                new FileSubject(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)[0])
                    .Exists.Should.BeTrue();

            [Test]
            public void False() =>
                new FileSubject("MissingFile.txt")
                    .Exists.Should.BeFalse();
        }

        [TestFixture]
        public class ReadAllText
        {
            [Test]
            public void ProviderName() =>
                new FileSubject("some.txt", "sut")
                    .ReadAllText().ProviderName.ToResultSubject().Should.Equal("sut.ReadAllText() => result");

            [Test]
            public void Executes()
            {
                using (var directoryFixture = DirectoryFixture.CreateUniqueDirectory())
                {
                    directoryFixture.CreateFile("1.txt", "some text");

                    new FileSubject(Path.Combine(directoryFixture.DirectoryPath, "1.txt"), "sut")
                        .ReadAllText().Should.Equal("some text");
                }
            }

            [Test]
            public void Throws_WhenFileNotFound()
            {
                var text = new FileSubject("missing.txt", "sut")
                    .ReadAllText();

                Assert.Throws<FileNotFoundException>(() =>
                    _ = text.Value);
            }
        }
    }
}
