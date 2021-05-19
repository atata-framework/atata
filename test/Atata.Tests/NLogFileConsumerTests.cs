using System;
using System.IO;
using Atata.Tests.DataProvision;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Atata.Tests
{
    public class NLogFileConsumerTests : UITestFixtureBase
    {
        [Test]
        public void ConfigureByDefault()
        {
            ConfigureBaseAtataContext()
                .AddNLogFileLogging()
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(DefaultAtataContextArtifactsDirectory.BuildPath(), $"{AtataContext.Current.TestName}.log"));
        }

        [Test]
        public void ConfigureWithFilePath()
        {
            using var directoryFixture = DirectoryFixture.CreateUniqueDirectory();
            string filePath = Path.Combine(directoryFixture.DirectoryPath, "test.log");

            ConfigureBaseAtataContext()
                .AddNLogFileLogging()
                    .WithFilePath(filePath)
                .Build();

            WriteLogMessageAndAssertItInFile(filePath);
        }

        [Test]
        public void ConfigureWithFilePathThatContainsVariables()
        {
            using var directoryFixture = DirectoryFixture.CreateUniqueDirectory();
            string filePath = Path.Combine(directoryFixture.DirectoryPath, "{test-name-sanitized}-{driver-alias}", "test.log");

            ConfigureBaseAtataContext()
                .AddNLogFileLogging()
                    .WithFilePath(filePath)
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(directoryFixture.DirectoryPath, $"{AtataContext.Current.TestNameSanitized}-{AtataContext.Current.DriverAlias}", "test.log"));
        }

        [Test]
        public void ConfigureWithFolderPath()
        {
            using var directoryFixture = DirectoryFixture.CreateUniqueDirectory();

            ConfigureBaseAtataContext()
                .AddNLogFileLogging()
                    .WithFolderPath(directoryFixture.DirectoryPath)
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(directoryFixture.DirectoryPath, NLogFileConsumer.DefaultFileName));
        }

        [Test]
        public void ConfigureWithFolderPathThatContainsVariables()
        {
            ConfigureBaseAtataContext()
                .AddNLogFileLogging()
                    .WithFolderPath("{artifacts}/1")
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(AtataContext.Current.Artifacts.FullName, "1", NLogFileConsumer.DefaultFileName));
        }

        [Test]
        public void ConfigureWithArtifactsFolderPath()
        {
            ConfigureBaseAtataContext()
                .AddNLogFileLogging()
                    .WithArtifactsFolderPath()
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(AtataContext.Current.Artifacts.FullName, NLogFileConsumer.DefaultFileName));
        }

        [Test]
        public void ConfigureWithFileName()
        {
            string fileName = Guid.NewGuid().ToString();

            ConfigureBaseAtataContext()
                .AddNLogFileLogging()
                    .WithFileName(fileName)
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(DefaultAtataContextArtifactsDirectory.BuildPath(), fileName));
        }

        private static void WriteLogMessageAndAssertItInFile(string filePath)
        {
            string testMessage = Guid.NewGuid().ToString();

            AtataContext.Current.Log.Info(testMessage);

            AssertThatFileShouldContainText(filePath, testMessage);
        }
    }
}
