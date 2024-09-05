namespace Atata.IntegrationTests;

public class ReportTests : WebDriverSessionTestSuite
{
    [Test]
    public void BulkLog()
    {
        var errorException = new InvalidOperationException("error");
        var fatalException = new InvalidOperationException("fatal");

        Go.To<OrdinaryPage>()
            .Report.Trace("tracemessage")
            .Report.Debug("debugmessage")
            .Report.Info("infomessage")
            .Report.Warn("warnmessage")
            .Report.Error(errorException, null)
            .Report.Error(errorException, "errormessage")
            .Report.Fatal(fatalException, null)
            .Report.Fatal(fatalException, "fatalmessage");

        VerifyLastLogEntries(
            (LogLevel.Trace, "tracemessage", null),
            (LogLevel.Debug, "debugmessage", null),
            (LogLevel.Info, "infomessage", null),
            (LogLevel.Warn, "warnmessage", null),
            (LogLevel.Error, null, errorException),
            (LogLevel.Error, "errormessage", errorException),
            (LogLevel.Fatal, null, fatalException),
            (LogLevel.Fatal, "fatalmessage", fatalException));
    }

    [Test]
    public void EdgeCaseMessages()
    {
        Go.To<OrdinaryPage>()
            .Report.Info("{")
            .Report.Info("}")
            .Report.Info(null);

        VerifyLastLogEntries(
            (LogLevel.Info, "{", null),
            (LogLevel.Info, "}", null),
            (LogLevel.Info, null, null));
    }

    [Test]
    public void PageSnapshot()
    {
        Go.To(new OrdinaryPage("Test"))
            .Report.PageSnapshot()
            .Report.PageSnapshot("sometitle");

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> Take page snapshot #01$",
            "^< Take page snapshot #01 \\(.*\\) >> \"01 Test page.mhtml\"$",
            "^> Take page snapshot #02 sometitle$",
            "^< Take page snapshot #02 sometitle \\(.*\\) >> \"02 Test page - sometitle.mhtml\"$");

        AtataContext.Current.Artifacts.Should.ContainFiles(
            "01 Test page.mhtml",
            "02 Test page - sometitle.mhtml");
    }

    [Test]
    public void Setup()
    {
        var page = Go.To<StubPage>();
        page.Report.Setup("TEST SETUP", x => x
            .IsTrue.Should.BeTrue());

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST SETUP",
            "^> Assert: IsTrue should be true$",
            "^< Assert: IsTrue should be true",
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

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST SETUP",
            "^> Assert: IsTrue should be true$",
            "^< Assert: IsTrue should be true",
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
        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST SETUP",
            "^> Assert: IsTrue should be true$",
            "^< Assert: IsTrue should be true",
            "^< TEST SETUP");
    }

    [Test]
    public void Step()
    {
        var page = Go.To<StubPage>();
        page.Report.Step("TEST STEP", x => x
            .IsTrue.Should.BeTrue());

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST STEP$",
            "^> Assert: IsTrue should be true$",
            "^< Assert: IsTrue should be true",
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

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST STEP$",
            "^> Assert: IsTrue should be true$",
            "^< Assert: IsTrue should be true",
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
        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST STEP$",
            "^> Assert: IsTrue should be true$",
            "^< Assert: IsTrue should be true",
            "^< TEST STEP");
    }

    public class Screenshot : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void ViewportVsFullPage()
        {
            var context = ConfigureAtataContextWithWebDriverSession().Build();

            var page = context.GetWebSession().Go.To<ScrollablePage>();

            ValueProvider<long, FileSubject> TakeScreenshotAndReturnItsSize(ScreenshotKind kind)
            {
                page.Report.Screenshot(kind);

                string fileName = context.Artifacts.Files.Value
                    .OrderByDescending(x => x.Object.CreationTimeUtc)
                    .First()
                    .Name;

                var file = context.Artifacts.Files.Single(x => x.Name == fileName).Should.Exist();
                return file.Length;
            }

            var defaultScreenshotSize = TakeScreenshotAndReturnItsSize(ScreenshotKind.Default);
            var viewportScreenshotSize = TakeScreenshotAndReturnItsSize(ScreenshotKind.Viewport);
            var fullPageScreenshotSize = TakeScreenshotAndReturnItsSize(ScreenshotKind.FullPage);

            viewportScreenshotSize.Should.Be(defaultScreenshotSize);
            fullPageScreenshotSize.Should.BeGreater((long)(viewportScreenshotSize * 1.5));
        }

        [Test]
        public void ViewportVsFullPage_ThroughConfiguration()
        {
            ValueProvider<long, FileSubject> TakeScreenshotAndReturnItsSize(Action<ScreenshotsWebDriverSessionOptions> screenshotsConfigurationAction)
            {
                var builder = ConfigureAtataContextWithWebDriverSession(
                    session => screenshotsConfigurationAction?.Invoke(session.Screenshots));

                using var context = builder.Build();

                string screenshotNameIndicator = Guid.NewGuid().ToString();
                context.GetWebDriverSession().Go.To<ScrollablePage>()
                    .Report.Screenshot(screenshotNameIndicator);

                var file = context.Artifacts.Files
                    .Single(x => x.Name.Value.Contains(screenshotNameIndicator)).Should.Exist();
                return file.Length;
            }

            var viewportScreenshotSize = TakeScreenshotAndReturnItsSize(x => x.UseWebDriverViewportStrategy());
            var fullPageScreenshotSize = TakeScreenshotAndReturnItsSize(x => x.UseFullPageOrViewportStrategy());

            fullPageScreenshotSize.Should.BeGreater((long)(viewportScreenshotSize * 1.5));
        }

        [Test]
        public void FilesAndLogEntries()
        {
            ConfigureAtataContextWithWebDriverSession().Build();

            Go.To(new OrdinaryPage("Test"))
                .Report.Screenshot()
                .Report.Screenshot("sometitle");

            VerifyLastLogMessagesMatch(
                minLogLevel: LogLevel.Trace,
                "^> Take screenshot #01$",
                "^< Take screenshot #01 \\(.*\\) >> \"01 Test page.png\"$",
                "^> Take screenshot #02 sometitle$",
                "^< Take screenshot #02 sometitle \\(.*\\) >> \"02 Test page - sometitle.png\"$");

            AtataContext.Current.Artifacts.Should.ContainFiles(
                "01 Test page.png",
                "02 Test page - sometitle.png");
        }
    }
}
