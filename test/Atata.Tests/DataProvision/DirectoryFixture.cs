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

        public static DirectoryFixture CreateUniqueDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

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

        public DirectoryFixture CreateDirectory(string directoryName)
        {
            Directory.CreateDirectory(Path.Combine(DirectoryPath, directoryName));

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
