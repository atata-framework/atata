using System;
using System.IO;

namespace Atata.Tests.DataProvision
{
    public class DirectoryFixture : IDisposable
    {
        private bool isDisposed;

        public DirectoryFixture(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public string DirectoryPath { get; }

        public string DirectoryName => Path.GetFileName(DirectoryPath);

        public static DirectoryFixture CreateUniqueDirectory() =>
            CreateUniqueDirectoryIn(Path.Combine(Path.GetTempPath(), "Atata"));

        public static DirectoryFixture CreateUniqueDirectoryIn(string parentDirectoryPath)
        {
            string path = Path.Combine(parentDirectoryPath, Guid.NewGuid().ToString());

            return new DirectoryFixture(path)
                .Create();
        }

        public DirectoryFixture Create()
        {
            Directory.CreateDirectory(DirectoryPath);

            return this;
        }

        public DirectoryFixture CreateFile(string fileName, string content = null)
        {
            File.WriteAllText(
                Path.Combine(DirectoryPath, fileName),
                content ?? fileName);

            return this;
        }

        public DirectoryFixture CreateFiles(params string[] fileNames)
        {
            foreach (string fileName in fileNames)
                CreateFile(fileName);

            return this;
        }

        public DirectoryFixture CreateDirectory(string directoryName)
        {
            Directory.CreateDirectory(Path.Combine(DirectoryPath, directoryName));

            return this;
        }

        public DirectoryFixture CreateDirectories(params string[] directoryNames)
        {
            foreach (string directoryName in directoryNames)
                CreateDirectory(directoryName);

            return this;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing && Directory.Exists(DirectoryPath))
                    Directory.Delete(DirectoryPath, true);

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
