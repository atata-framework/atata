namespace Atata.IntegrationTests;

public class ReportTests : WebDriverSessionTestSuite
{
    [Test]
    public void BulkLog()
    {
        var errorException = new InvalidOperationException("error");
        var fatalException = new InvalidOperationException("fatal");

        Go.To<OrdinaryPage>()
            .Report.Trace("trace message")
            .Report.Debug("debug message")
            .Report.Info("informational message")
            .Report.Warn("warning message")
            .Report.Error(errorException)
            .Report.Error(errorException, "error message")
            .Report.Fatal(fatalException)
            .Report.Fatal(fatalException, "fatal message");

        VerifyLastLogEntries(
            (LogLevel.Trace, "trace message", null),
            (LogLevel.Debug, "debug message", null),
            (LogLevel.Info, "informational message", null),
            (LogLevel.Warn, "warning message", null),
            (LogLevel.Error, null, errorException),
            (LogLevel.Error, "error message", errorException),
            (LogLevel.Fatal, null, fatalException),
            (LogLevel.Fatal, "fatal message", fatalException));
    }

    [Test]
    public void EdgeCaseMessages()
    {
        Go.To<OrdinaryPage>()
            .Report.Info("{")
            .Report.Info("}")
            .Report.Info(null!);

        VerifyLastLogEntries(
            (LogLevel.Info, "{", null),
            (LogLevel.Info, "}", null),
            (LogLevel.Info, null, null));
    }

    [Test]
    public void Setup()
    {
        var page = Go.To<StubPage>();
        page.Report.Setup("TEST SETUP", x => x
            .IsTrue.Should.BeTrue());

        VerifyLastLogNestingTextsWithMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST SETUP$",
            "^- > Assert: IsTrue should be true$",
            "^- < Assert: IsTrue should be true",
            "^< TEST SETUP");
    }

    [Test]
    public async Task SetupAsync()
    {
        var page = Go.To<StubPage>();
        await page.Report.SetupAsync("TEST SETUP", async x =>
        {
            x.IsTrue.Should.BeTrue();
            await Task.CompletedTask;
        });

        VerifyLastLogNestingTextsWithMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST SETUP$",
            "^- > Assert: IsTrue should be true$",
            "^- < Assert: IsTrue should be true",
            "^< TEST SETUP");
    }

    [Test]
    public async Task SetupAsyncWithResult()
    {
        var page = Go.To<StubPage>();
        string result = await page.Report.SetupAsync("TEST SETUP", async x =>
        {
            x.IsTrue.Should.BeTrue();
            return await Task.FromResult("ok");
        });

        result.Should().Be("ok");
        VerifyLastLogNestingTextsWithMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST SETUP$",
            "^- > Assert: IsTrue should be true$",
            "^- < Assert: IsTrue should be true",
            "^< TEST SETUP");
    }

    [Test]
    public void Step()
    {
        var page = Go.To<StubPage>();
        page.Report.Step("TEST STEP", x => x
            .IsTrue.Should.BeTrue());

        VerifyLastLogNestingTextsWithMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST STEP$",
            "^- > Assert: IsTrue should be true$",
            "^- < Assert: IsTrue should be true",
            "^< TEST STEP");
    }

    [Test]
    public async Task StepAsync()
    {
        var page = Go.To<StubPage>();
        await page.Report.StepAsync("TEST STEP", async x =>
        {
            x.IsTrue.Should.BeTrue();
            await Task.CompletedTask;
        });

        VerifyLastLogNestingTextsWithMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST STEP$",
            "^- > Assert: IsTrue should be true$",
            "^- < Assert: IsTrue should be true",
            "^< TEST STEP");
    }

    [Test]
    public async Task StepAsyncWithResult()
    {
        var page = Go.To<StubPage>();
        string result = await page.Report.StepAsync("TEST STEP", async x =>
        {
            x.IsTrue.Should.BeTrue();
            return await Task.FromResult("ok");
        });

        result.Should().Be("ok");
        VerifyLastLogNestingTextsWithMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST STEP$",
            "^- > Assert: IsTrue should be true$",
            "^- < Assert: IsTrue should be true",
            "^< TEST STEP");
    }
}
