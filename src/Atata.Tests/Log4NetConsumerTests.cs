using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NUnit.Framework;

namespace Atata.Tests
{
    public class Log4NetConsumerTests : UITestFixtureBase
    {
        public override void TearDown()
        {
            base.TearDown();

            foreach (var repository in log4net.LogManager.GetAllRepositories())
            {
                repository.ResetConfiguration();
                repository.Shutdown();
            }
        }

        [Test]
        public void Log4NetConsumer()
        {
            var logRepository = log4net.LogManager.CreateRepository("Log4NetConsumer");
            XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "log4net.config")));

            ConfigureBaseAtataContext().
                AddLog4NetLogging(logRepository.Name, "AtataLog4Net").
                Build();

            string testMessage = Guid.NewGuid().ToString();
            AtataContext.Current.Log.Info(testMessage);

            string filePath = logRepository.GetAppenders().OfType<FileAppender>().First().File;

            FileAssert.Exists(filePath);
            string fileContent = File.ReadAllText(filePath);
            fileContent.Should().Contain(testMessage);
        }

        [Test]
        public void Log4NetConsumer_MissingRepository()
        {
            string repositoryName = "AtataRepo";

            var exception = Assert.Throws<LogException>(() =>
                ConfigureBaseAtataContext().
                    AddLog4NetLogging(repositoryName, "AtataLogger").
                    Build());

            exception.Message.Should().Be($"Repository [{repositoryName}] is NOT defined.");
        }

        [Test]
        public void Log4NetConsumer_UnconfiguredRepository()
        {
            var repository = log4net.LogManager.CreateRepository("Log4NetConsumerUnconfiguredRepo");

            var exception = Assert.Throws<InvalidOperationException>(() =>
                ConfigureBaseAtataContext().
                    AddLog4NetLogging(repository.Name, "AtataLogger").
                    Build());

            exception.Message.Should().Be($"Log4Net '{repository.Name}' repository is not configured.");
        }

        [Test]
        public void Log4NetConsumer_UnconfiguredLogger()
        {
            var incorrectLoggerName = "AtataatatA";
            var logRepository = log4net.LogManager.CreateRepository("AtataRepo");
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "log4net.config")));

            ConfigureBaseAtataContext().
                AddLog4NetLogging(logRepository.Name, incorrectLoggerName).
                Build();

            string testMessage = Guid.NewGuid().ToString();
            AtataContext.Current.Log.Info(testMessage);

            string filePath = logRepository.GetAppenders().OfType<FileAppender>().
                First(x => x.Name == "LogFileAppender2").File;

            FileAssert.Exists(filePath);
            string fileContent = File.ReadAllText(filePath);
            Assert.That(fileContent, Does.Contain(testMessage));
        }
    }
}
