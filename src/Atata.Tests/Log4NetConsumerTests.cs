using System;
using System.IO;
using log4net.Appender;
using log4net.Core;
using NUnit.Framework;

namespace Atata.Tests
{
    public class Log4NetConsumerTests : UITestFixtureBase
    {
        [Test]
        public void Log4NetConsumer()
        {
            var logRepository = log4net.LogManager.CreateRepository("Log4NetConsumer");
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            ConfigureBaseAtataContext().
                AddLog4NetLogging(logRepository.Name, "AtataLog4Net").
                Build();

            string testMessage = Guid.NewGuid().ToString();
            AtataContext.Current.Log.Info(testMessage);

            string filePath = null;
            foreach (IAppender appender in logRepository.GetAppenders())
            {
                Type t = appender.GetType();
                // Get the file name from the first FileAppender found and return
                if (t.Equals(typeof(FileAppender)) || t.Equals(typeof(RollingFileAppender)))
                {
                    filePath = ((FileAppender)appender).File;
                    break;
                }
            }

            FileAssert.Exists(filePath);

            string fileContent = File.ReadAllText(filePath);

            Assert.That(fileContent, Does.Contain(testMessage));
        }

        [Test]
        public void Log4NetConsumerIncorrectParams()
        {
            var repoName = "AtataRepo";
            var ex = Assert.Throws<LogException>(() =>
                ConfigureBaseAtataContext().
                    AddLog4NetLogging(repoName, "AtataLogger").
                    Build());
            Assert.That(ex.Message == $"Repository [{repoName}] is NOT defined.");
        }

        [Test]
        public void Log4NetConsumerUnconfiguredRepo()
        {
            var logRepository = log4net.LogManager.CreateRepository("Log4NetConsumerUnconfiguredRepo");
            var ex = Assert.Throws<ArgumentException>(() =>
                ConfigureBaseAtataContext().
                    AddLog4NetLogging(logRepository.Name, "AtataLogger").
                    Build());
            Assert.That(ex.Message == $"Log4Net repository '{logRepository.Name}' is not configured.");
        }

        [Test]
        public void Log4NetConsumerNotConfiguredLogger()
        {
            var incorrectLoggerName = "AtataatatA";
            var logRepository = log4net.LogManager.CreateRepository("AtataRepo");
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            ConfigureBaseAtataContext().
                            AddLog4NetLogging(logRepository.Name, incorrectLoggerName).
                            Build();

            string testMessage = Guid.NewGuid().ToString();
            AtataContext.Current.Log.Info(testMessage);

            string filePath = null;
            foreach (IAppender appender in logRepository.GetAppenders())
            {
                Type t = appender.GetType();
                if ((t.Equals(typeof(FileAppender)) || t.Equals(typeof(RollingFileAppender))) && appender.Name == "LogFileAppender2")
                {
                    filePath = ((FileAppender)appender).File;
                    break;
                }
            }

            FileAssert.Exists(filePath);
            string fileContent = File.ReadAllText(filePath);
            Assert.That(fileContent, Does.Contain(testMessage));
        }

        [SetUp]
        public void Cleanup()
        {
            foreach (var repo in log4net.LogManager.GetAllRepositories())
            {
                repo.ResetConfiguration();
                repo.Shutdown();
            }
        }
    }
}
