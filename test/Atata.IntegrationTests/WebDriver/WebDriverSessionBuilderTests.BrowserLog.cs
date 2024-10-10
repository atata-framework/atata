namespace Atata.IntegrationTests.WebDriver;

public partial class WebDriverSessionBuilderTests
{
    public sealed class BrowserLog : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void UseLog()
        {
            BuildAtataContextWithWebDriverSession(
                x => x.BrowserLogs.UseLog());

            GoToTestPageAndVerifyLogs();
        }

        [Test]
        public void UseLog_AfterRestartDriver()
        {
            BuildAtataContextWithWebDriverSession(
                x => x.BrowserLogs.UseLog());
            Go.To<OrdinaryPage>(url: "/input");
            AtataContext.Current.GetWebDriverSession().RestartDriver();

            GoToTestPageAndVerifyLogs();
        }

        [Test]
        public void UseMinLevelOfWarning_Warn()
        {
            BuildAtataContextWithWebDriverSession(
                x => x.BrowserLogs.UseMinLevelOfWarning(LogLevel.Warn));
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
            BuildAtataContextWithWebDriverSession(
                x => x.BrowserLogs.UseMinLevelOfWarning(LogLevel.Error));
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

            // Wait a bit for browser logs to be recorded.
            Thread.Sleep(500);

            AtataContext.Current.Dispose();

            var logRecords = CurrentLog.GetSnapshot();
            logRecords.Should().ContainSingle(
                x => x.Level == LogLevel.Debug &&
                x.ExternalSource == "Browser" &&
                x.Message.Contains("console debug log entry"));
            logRecords.Should().ContainSingle(
                x => x.Level == LogLevel.Info &&
                x.ExternalSource == "Browser" &&
                x.Message.Contains("console info log entry"));
            logRecords.Should().ContainSingle(
                x => x.Level == LogLevel.Warn &&
                x.ExternalSource == "Browser" &&
                x.Message.Contains("console warn log entry"));
            logRecords.Should().ContainSingle(
                x => x.Level == LogLevel.Error &&
                x.ExternalSource == "Browser" &&
                x.Message.Contains("console error log entry"));
            logRecords.Should().ContainSingle(
                x => x.Level == LogLevel.Error &&
                x.ExternalSource == "Browser" &&
                x.Message.Contains("thrown error"));
        }
    }
}
