namespace Atata.IntegrationTests.Context;

public class AtataContextBrowserLogsTests : UITestFixtureBase
{
    [Test]
    public void Log()
    {
        ConfigureBaseAtataContext()
            .BrowserLogs.UseLog()
            .Build();

        GoToTestPageAndVerifyLogs();
    }

    [Test]
    public void Log_AfterRestartDriver()
    {
        ConfigureBaseAtataContext()
            .BrowserLogs.UseLog()
            .Build();
        Go.To<OrdinaryPage>(url: "/input");
        AtataContext.Current.RestartDriver();

        GoToTestPageAndVerifyLogs();
    }

    [Test]
    public void UseMinLevelOfWarning_Warn()
    {
        ConfigureBaseAtataContext()
            .BrowserLogs.UseMinLevelOfWarning(LogLevel.Warn)
            .Build();
        Go.To<OrdinaryPage>(url: "/browserlogs");

        Subject.Invoking(() => AtataContext.Current.Dispose())
            .Should.Throw<AggregateAssertionException>()
            .SubjectOf(x => x.Results)
                .Should.HaveCount(3)
                .Select(x => x.Status).Should.ConsistOnlyOf(AssertionStatus.Warning)
                .ValueOf(x => x[0].Message).Should.ContainAll(
                    "Unexpected browser log warning on \"<ordinary>\" page",
                    "console warn log entry")
                .ValueOf(x => x[1].Message).Should.ContainAll(
                    "Unexpected browser log error on \"<ordinary>\" page",
                    "console error log entry")
                .ValueOf(x => x[2].Message).Should.ContainAll(
                    "Unexpected browser log error on \"<ordinary>\" page",
                    "thrown error");
    }

    [Test]
    public void UseMinLevelOfWarning_Error()
    {
        ConfigureBaseAtataContext()
            .BrowserLogs.UseMinLevelOfWarning(LogLevel.Error)
            .Build();
        Go.To<OrdinaryPage>(url: "/browserlogs")
            .WaitSeconds(1);

        Subject.Invoking(() => AtataContext.Current.Dispose())
            .Should.Throw<AggregateAssertionException>()
            .SubjectOf(x => x.Results)
                .Should.HaveCount(2)
                .Select(x => x.Status).Should.ConsistOnlyOf(AssertionStatus.Warning)
                .ValueOf(x => x[0].Message).Should.ContainAll(
                    "Unexpected browser log error on \"<ordinary>\" page",
                    "console error log entry")
                .ValueOf(x => x[1].Message).Should.ContainAll(
                    "Unexpected browser log error on \"<ordinary>\" page",
                    "thrown error");
    }

    private void GoToTestPageAndVerifyLogs()
    {
        Go.To<OrdinaryPage>(url: "/browserlogs");

        AtataContext.Current.Dispose();

        LogEntries.Should().ContainSingle(
            x => x.Level == LogLevel.Trace &&
            x.Message.Contains("DEBUG") &&
            x.Message.Contains("console debug log entry"));
        LogEntries.Should().ContainSingle(
            x => x.Level == LogLevel.Trace &&
            x.Message.Contains("INFO") &&
            x.Message.Contains("console info log entry"));
        LogEntries.Should().ContainSingle(
            x => x.Level == LogLevel.Trace &&
            x.Message.Contains("WARN") &&
            x.Message.Contains("console warn log entry"));
        LogEntries.Should().ContainSingle(
            x => x.Level == LogLevel.Trace &&
            x.Message.Contains("ERROR") &&
            x.Message.Contains("console error log entry"));
        LogEntries.Should().ContainSingle(
            x => x.Level == LogLevel.Trace &&
            x.Message.Contains("ERROR") &&
            x.Message.Contains("thrown error"));
    }
}
