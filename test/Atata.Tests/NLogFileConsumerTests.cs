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
                .LogConsumers.AddNLogFile()
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
                .LogConsumers.AddNLogFile()
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
                .LogConsumers.AddNLogFile()
                    .WithFilePath(filePath)
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(directoryFixture.DirectoryPath, $"{AtataContext.Current.TestNameSanitized}-{AtataContext.Current.DriverAlias}", "test.log"));
        }

        [Test]
        public void ConfigureWithDirectoryPath()
        {
            using var directoryFixture = DirectoryFixture.CreateUniqueDirectory();

            ConfigureBaseAtataContext()
                .LogConsumers.AddNLogFile()
                    .WithDirectoryPath(directoryFixture.DirectoryPath)
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(directoryFixture.DirectoryPath, NLogFileConsumer.DefaultFileName));
        }

        [Test]
        public void ConfigureWithDirectoryPathThatContainsVariables()
        {
            ConfigureBaseAtataContext()
                .LogConsumers.AddNLogFile()
                    .WithDirectoryPath("{artifacts}/1")
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(AtataContext.Current.Artifacts.FullName, "1", NLogFileConsumer.DefaultFileName));
        }

        [Test]
        public void ConfigureWithArtifactsDirectoryPath()
        {
            ConfigureBaseAtataContext()
                .LogConsumers.AddNLogFile()
                    .WithArtifactsDirectoryPath()
                .Build();

            WriteLogMessageAndAssertItInFile(
                Path.Combine(AtataContext.Current.Artifacts.FullName, NLogFileConsumer.DefaultFileName));
        }

        [Test]
        public void ConfigureWithFileName()
        {
            string fileName = Guid.NewGuid().ToString();

            ConfigureBaseAtataContext()
                .LogConsumers.AddNLogFile()
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
