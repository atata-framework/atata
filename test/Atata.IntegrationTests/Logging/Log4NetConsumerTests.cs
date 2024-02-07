using log4net.Appender;
using log4net.Config;
using log4net.Core;

namespace Atata.IntegrationTests.Logging;

public class Log4NetConsumerTests : UITestFixtureBase
{
    private const string InfoLoggerName = "InfoLogger";

    private static FileInfo ConfigFileInfo =>
        new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));

    private static string LogsDirectory =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4NetLogs");

    private static string TraceLogFilePath =>
        Path.Combine(LogsDirectory, "Trace.log");

    public override void TearDown()
    {
        base.TearDown();

        foreach (var repository in log4net.LogManager.GetAllRepositories())
            repository.Shutdown();

        if (Directory.Exists(LogsDirectory))
            Directory.Delete(LogsDirectory, recursive: true);
    }

    [Test]
    public void WithDefaultConfiguration()
    {
        XmlConfigurator.Configure(ConfigFileInfo);

        ConfigureBaseAtataContext()
            .LogConsumers.AddLog4Net()
            .Build();

        string traceTestMessage = Guid.NewGuid().ToString();
        string debugTestMessage = Guid.NewGuid().ToString();
        string infoTestMessage = Guid.NewGuid().ToString();

        AtataContext.Current.Log.Trace(traceTestMessage);
        AtataContext.Current.Log.Debug(debugTestMessage);
        AtataContext.Current.Log.Info(infoTestMessage);

        AssertThatFileShouldContainText(TraceLogFilePath, traceTestMessage, debugTestMessage, infoTestMessage);
    }

    [Test]
    public void WithRepositoryUsingInfoLevel()
    {
        var logRepository = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
        XmlConfigurator.Configure(logRepository, ConfigFileInfo);

        ConfigureBaseAtataContext()
            .LogConsumers.AddLog4Net(logRepository.Name, InfoLoggerName)
            .Build();

        string traceTestMessage = Guid.NewGuid().ToString();
        string debugTestMessage = Guid.NewGuid().ToString();
        string infoTestMessage = Guid.NewGuid().ToString();

        AtataContext.Current.Log.Trace(traceTestMessage);
        AtataContext.Current.Log.Debug(debugTestMessage);
        AtataContext.Current.Log.Info(infoTestMessage);

        var fileAppenders = logRepository.GetAppenders().OfType<FileAppender>().ToArray();
        fileAppenders.Should().HaveCount(2);

        foreach (var fileAppender in fileAppenders)
        {
            AssertThatFileShouldContainText(fileAppender.File, infoTestMessage);
            AssertThatFileShouldNotContainText(fileAppender.File, traceTestMessage, debugTestMessage);
        }
    }

    [Test]
    public void WithMissingRepository()
    {
        string repositoryName = "MissingRepository";

        var exception = Assert.Throws<LogException>(() =>
            ConfigureBaseAtataContext()
                .LogConsumers.AddLog4Net(repositoryName, InfoLoggerName)
                .Build());

        exception.Message.Should().Be($"Repository [{repositoryName}] is NOT defined.");
    }

    [Test]
    public void WithUnconfiguredRepository()
    {
        var repository = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());

        ConfigureBaseAtataContext()
            .LogConsumers.AddLog4Net(repository.Name, InfoLoggerName)
            .Build();
    }
}
