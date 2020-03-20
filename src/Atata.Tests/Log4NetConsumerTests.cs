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
        private FileInfo ConfigFileInfo =>
            new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));

        private string LogsFolder =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Log4Net");

        public override void TearDown()
        {
            base.TearDown();

            foreach (var repository in log4net.LogManager.GetAllRepositories())
            {
                repository.ResetConfiguration();
                repository.Shutdown();
            }

            if (Directory.Exists(LogsFolder))
                Directory.Delete(LogsFolder, recursive: true);
        }

        [Test]
        public void Log4NetConsumer()
        {
            var logRepository = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
            XmlConfigurator.Configure(logRepository, ConfigFileInfo);

            ConfigureBaseAtataContext().
                AddLog4NetLogging(logRepository.Name, "DebugLogger").
                Build();

            string testMessage = Guid.NewGuid().ToString();

            AtataContext.Current.Log.Info(testMessage);

            var fileAppenders = logRepository.GetAppenders().OfType<FileAppender>().ToArray();
            fileAppenders.Should().HaveCount(2);

            foreach (FileAppender fileAppender in fileAppenders)
                AssertThatFileContainsText(fileAppender.File, testMessage);
        }

        [Test]
        public void Log4NetConsumer_WithMissingRepository()
        {
            string repositoryName = "MissingRepository";

            var exception = Assert.Throws<LogException>(() =>
                ConfigureBaseAtataContext().
                    AddLog4NetLogging(repositoryName, "DebugLogger").
                    Build());

            exception.Message.Should().Be($"Repository [{repositoryName}] is NOT defined.");
        }

        [Test]
        public void Log4NetConsumer_WithUnconfiguredRepository()
        {
            var repository = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());

            var exception = Assert.Throws<InvalidOperationException>(() =>
                ConfigureBaseAtataContext().
                    AddLog4NetLogging(repository.Name, "DebugLogger").
                    Build());

            exception.Message.Should().Be($"Log4Net '{repository.Name}' repository is not configured.");
        }

        [Test]
        public void Log4NetConsumer_WithUnconfiguredLogger()
        {
            var logRepository = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
            XmlConfigurator.Configure(logRepository, ConfigFileInfo);

            ConfigureBaseAtataContext().
                AddLog4NetLogging(logRepository.Name, "MissingLogger").
                Build();

            string testMessage = Guid.NewGuid().ToString();
            AtataContext.Current.Log.Info(testMessage);

            var fileAppender = logRepository.GetAppenders().OfType<FileAppender>().First(x => x.Name == "DefaultInfoFileAppender");
            AssertThatFileContainsText(fileAppender.File, testMessage);
        }
    }
}
