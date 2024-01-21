namespace Atata.IntegrationTests;

public class ReportTests : UITestFixture
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
            "^< Take page snapshot #01 \\(.*\\) >> \"01 - Test page.mhtml\"$",
            "^> Take page snapshot #02 - sometitle$",
            "^< Take page snapshot #02 - sometitle \\(.*\\) >> \"02 - Test page - sometitle.mhtml\"$");

        AtataContext.Current.Artifacts.Should.ContainFiles(
            "01 - Test page.mhtml",
            "02 - Test page - sometitle.mhtml");
    }

    [Test]
    public void Setup()
    {
        Go.To(new OrdinaryPage("Test"))
            .Report.Setup("TEST SETUP", x => x
                .PageTitle.Should.Not.BeEmpty());

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST SETUP",
            "^> Assert: title should not be empty$",
            "^< Assert: title should not be empty",
            "^< TEST SETUP");
    }

    [Test]
    public void Step()
    {
        Go.To(new OrdinaryPage("Test"))
            .Report.Step("TEST STEP", x => x
                .PageTitle.Should.Not.BeEmpty());

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> TEST STEP$",
            "^> Assert: title should not be empty$",
            "^< Assert: title should not be empty",
            "^< TEST STEP");
    }

    public class Screenshot : UITestFixtureBase
    {
        [Test]
        public void ViewportVsFullPage()
        {
            var context = ConfigureBaseAtataContext()
                .ScreenshotConsumers.AddFile()
                .Build();

            var page = context.Go.To<ScrollablePage>();

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
            ValueProvider<long, FileSubject> TakeScreenshotAndReturnItsSize(Action<ScreenshotsAtataContextBuilder> screenshotsConfigurationAction)
            {
                var builder = ConfigureBaseAtataContext()
                    .ScreenshotConsumers.AddFile();
                screenshotsConfigurationAction?.Invoke(builder.Screenshots);
                using var context = builder.Build();

                string screenshotNameIndicator = Guid.NewGuid().ToString();
                context.Go.To<ScrollablePage>()
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
        public void LogEntries_WithoutScreenshotConsumer()
        {
            ConfigureBaseAtataContext().Build();

            Go.To<OrdinaryPage>()
                .Report.Screenshot()
                .Report.Screenshot("sometitle");

            VerifyLastLogMessagesContain(
                minLogLevel: LogLevel.Trace,
                "Go to");
        }

        [Test]
        public new void LogEntries()
        {
            ConfigureBaseAtataContext().Build();
            MockScreenshotConsumer screenshotConsumer = new();

            AtataContext.Current.ScreenshotTaker.AddConsumer(screenshotConsumer);

            Go.To<OrdinaryPage>()
                .Report.Screenshot()
                .Report.Screenshot("sometitle");

            VerifyLastLogMessages(
                minLogLevel: LogLevel.Trace,
                "Take screenshot #01",
                "Take screenshot #02 - sometitle");

            screenshotConsumer.Items.Should().HaveCount(2);
            screenshotConsumer.Items[0].Number.Should().Be(1);
            screenshotConsumer.Items[0].Title.Should().BeNull();
            screenshotConsumer.Items[1].Number.Should().Be(2);
            screenshotConsumer.Items[1].Title.Should().Be("sometitle");
        }

        private sealed class MockScreenshotConsumer : IScreenshotConsumer
        {
            public List<ScreenshotInfo> Items { get; } = [];

            public void Take(ScreenshotInfo screenshotInfo) =>
                Items.Add(screenshotInfo);
        }
    }
}
