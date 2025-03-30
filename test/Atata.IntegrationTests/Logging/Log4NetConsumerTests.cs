#pragma warning disable CS0618 // Type or member is obsolete

using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Util;

namespace Atata.IntegrationTests.Logging;

public sealed class Log4NetConsumerTests : TestSuiteBase
{
    private const string InfoLoggerName = "InfoLogger";

    private static FileInfo ConfigFileInfo =>
        new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));

    private static string LogsDirectory =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4NetLogs");

    private static string TraceLogFilePath =>
        Path.Combine(LogsDirectory, "Trace.log");

    [OneTimeSetUp]
    public void SetUpTestSuite() =>
        SystemInfo.NullText = string.Empty;

    [TearDown]
    public void TearDownTest()
    {
        foreach (var repository in log4net.LogManager.GetAllRepositories())
            repository.Shutdown();

        if (Directory.Exists(LogsDirectory))
            Directory.Delete(LogsDirectory, recursive: true);
    }

    [Test]
    public void WithDefaultConfiguration()
    {
        // Arrange
        XmlConfigurator.Configure(ConfigFileInfo);

        var builder = ConfigureSessionlessAtataContext();
        builder.LogConsumers.AddLog4Net();
        var context = builder.Build();

        // Act
        string traceTestMessage = Guid.NewGuid().ToString();
        string debugTestMessage = Guid.NewGuid().ToString();
        string infoTestMessage = Guid.NewGuid().ToString();

        context.Log.Trace(traceTestMessage);
        context.Log.Debug(debugTestMessage);
        context.Log.Info(infoTestMessage);

        // Assert
        AssertThatFileShouldContainText(TraceLogFilePath, traceTestMessage, debugTestMessage, infoTestMessage);
    }

    [Test]
    public void WithRepositoryUsingInfoLevel()
    {
        // Arrange
        var logRepository = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
        XmlConfigurator.Configure(logRepository, ConfigFileInfo);

        var builder = ConfigureSessionlessAtataContext();
        builder.LogConsumers.AddLog4Net(x => x
            .WithRepositoryName(logRepository.Name)
            .WithLoggerName(InfoLoggerName));
        var context = builder.Build();

        // Act
        string traceTestMessage = Guid.NewGuid().ToString();
        string debugTestMessage = Guid.NewGuid().ToString();
        string infoTestMessage = Guid.NewGuid().ToString();

        context.Log.Trace(traceTestMessage);
        context.Log.Debug(debugTestMessage);
        context.Log.Info(infoTestMessage);

        // Assert
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
        // Arrange
        var builder = ConfigureSessionlessAtataContext();
        builder.LogConsumers.AddLog4Net(x => x
            .WithRepositoryName("MissingRepository")
            .WithLoggerName(InfoLoggerName));

        // Act // Assert
        var exception = Assert.Throws<LogException>(() =>
            builder.Build());

        exception.Message.Should().Be("Repository [MissingRepository] is NOT defined.");
    }

    [Test]
    public void WithUnconfiguredRepository()
    {
        // Arrange
        var repository = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());

        var builder = ConfigureSessionlessAtataContext();
        builder.LogConsumers.AddLog4Net(x => x
            .WithRepositoryName(repository.Name)
            .WithLoggerName(InfoLoggerName));

        // Act // Assert
        Assert.DoesNotThrow(() =>
            builder.Build());
    }
}
