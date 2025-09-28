namespace Atata.IntegrationTests.WebDriver;

public static partial class WebDriverSessionTests
{
    public sealed class Report : WebDriverSessionTestSuiteBase
    {
        public sealed class PageSnapshot : WebDriverSessionTestSuiteBase
        {
            [Test]
            public void FileIsSaved()
            {
                // Arrange
                var context = BuildAtataContextWithWebDriverSession();
                var session = context.Sessions.Get<WebDriverSession>();
                session.Go.To<InputPage>();

                // Act
                session.Report.PageSnapshot();

                // Assert
                context.Artifacts.Should.ContainFile("001-Input_page.mhtml");
            }

            [Test]
            public void FilesAreSavedAndLogEntriesAreRecorded()
            {
                // Arrange
                var context = BuildAtataContextWithWebDriverSession();
                var session = context.Sessions.Get<WebDriverSession>();
                session.Go.To(new OrdinaryPage("Test"));

                // Act
                session
                    .Report.PageSnapshot()
                    .Report.PageSnapshot("Some title");

                // Assert
                VerifyLastLogNestingTextsWithMessagesMatch(
                    minLogLevel: LogLevel.Trace,
                    "^> Take page snapshot #1$",
                    "^< Take page snapshot #1 \\(.*\\) >> Artifacts\\.Files\\[\"001-Test_page.mhtml\"\\]$",
                    "^> Take page snapshot #2 Some title$",
                    "^< Take page snapshot #2 Some title \\(.*\\) >> Artifacts\\.Files\\[\"002-Test_page-Some_title.mhtml\"\\]$");

                context.Artifacts.Should.ContainFiles(
                    "001-Test_page.mhtml",
                    "002-Test_page-Some_title.mhtml");
            }

            [Test]
            public void FilesAreSavedAndLogEntriesAreRecorded_WhenUseFileNameTemplateWithSessionId()
            {
                // Arrange
                var context = BuildAtataContextWithWebDriverSession(
                    x => x.PageSnapshots.UseFileNameTemplateWithSessionId());
                var session = context.Sessions.Get<WebDriverSession>();
                session.Go.To(new OrdinaryPage("Test"));

                // Act
                session
                    .Report.PageSnapshot()
                    .Report.PageSnapshot("Some title");

                // Assert
                VerifyLastLogNestingTextsWithMessagesMatch(
                    minLogLevel: LogLevel.Trace,
                    "^> Take page snapshot #1$",
                    $"^< Take page snapshot #1 \\(.*\\) >> Artifacts\\.Files\\[\"001-{session.Id}-Test_page.mhtml\"\\]$",
                    "^> Take page snapshot #2 Some title$",
                    $"^< Take page snapshot #2 Some title \\(.*\\) >> Artifacts\\.Files\\[\"002-{session.Id}-Test_page-Some_title.mhtml\"\\]$");

                context.Artifacts.Should.ContainFiles(
                    $"001-{session.Id}-Test_page.mhtml",
                    $"002-{session.Id}-Test_page-Some_title.mhtml");
            }
        }

        public sealed class Screenshot : WebDriverSessionTestSuiteBase
        {
            [Test]
            public void FileIsSaved()
            {
                // Arrange
                var context = BuildAtataContextWithWebDriverSession();
                var session = context.Sessions.Get<WebDriverSession>();
                session.Go.To<InputPage>();

                // Act
                session.Report.Screenshot();

                // Assert
                context.Artifacts.Should.ContainFile("001-Input_page.png");
            }

            [Test]
            public void ViewportVsFullPage()
            {
                var context = BuildAtataContextWithWebDriverSession();

                var page = context.Sessions.Get<WebDriverSession>().Go.To<ScrollablePage>();

                long TakeScreenshotAndReturnItsSize(ScreenshotKind kind)
                {
                    page.Report.Screenshot(kind);

                    string fileName = context.Artifacts.Files.Value
                        .OrderByDescending(x => x.Object.CreationTimeUtc)
                        .First()
                        .Name;

                    var file = context.Artifacts.Files.Single(x => x.Name == fileName).Should.Exist();
                    return file.Length;
                }

                long defaultScreenshotSize = TakeScreenshotAndReturnItsSize(ScreenshotKind.Default);
                long viewportScreenshotSize = TakeScreenshotAndReturnItsSize(ScreenshotKind.Viewport);
                long fullPageScreenshotSize = TakeScreenshotAndReturnItsSize(ScreenshotKind.FullPage);

                viewportScreenshotSize.Should().Be(defaultScreenshotSize);
                fullPageScreenshotSize.Should().BeGreaterThan((long)(viewportScreenshotSize * 1.5));
            }

            [Test]
            public void ViewportVsFullPage_ThroughConfiguration()
            {
                long TakeScreenshotAndReturnItsSize(Action<ScreenshotsWebDriverSessionBuilder> screenshotsConfigurationAction)
                {
                    var builder = ConfigureAtataContextWithWebDriverSession(
                        session => screenshotsConfigurationAction?.Invoke(session.Screenshots));

                    using var context = builder.Build();

                    string screenshotNameIndicator = Guid.NewGuid().ToString();
                    context.Sessions.Get<WebDriverSession>().Go.To<ScrollablePage>()
                        .Report.Screenshot(screenshotNameIndicator);

                    var file = context.Artifacts.Files
                        .Single(x => x.Name.Value.Contains(screenshotNameIndicator)).Should.Exist();
                    return file.Length.Value;
                }

                long viewportScreenshotSize = TakeScreenshotAndReturnItsSize(x => x.UseWebDriverViewportStrategy());
                long fullPageScreenshotSize = TakeScreenshotAndReturnItsSize(x => x.UseFullPageOrViewportStrategy());

                fullPageScreenshotSize.Should().BeGreaterThan((long)(viewportScreenshotSize * 1.5));
            }

            [Test]
            public void FilesAreSavedAndLogEntriesAreRecorded()
            {
                // Arrange
                var context = BuildAtataContextWithWebDriverSession();
                var session = context.Sessions.Get<WebDriverSession>();
                session.Go.To(new OrdinaryPage("Test"));

                // Act
                session
                    .Report.Screenshot()
                    .Report.Screenshot("some title");

                // Assert
                VerifyLastLogNestingTextsWithMessagesMatch(
                    minLogLevel: LogLevel.Trace,
                    "^> Take screenshot #1$",
                    "^< Take screenshot #1 \\(.*\\) >> Artifacts\\.Files\\[\"001-Test_page.png\"\\]$",
                    "^> Take screenshot #2 some title$",
                    "^< Take screenshot #2 some title \\(.*\\) >> Artifacts\\.Files\\[\"002-Test_page-some_title.png\"\\]$");

                context.Artifacts.Should.ContainFiles(
                    $"001-Test_page.png",
                    $"002-Test_page-some_title.png");
            }

            [Test]
            public void FilesAreSavedAndLogEntriesAreRecorded_WhenUseFileNameTemplateWithSessionId()
            {
                // Arrange
                var context = BuildAtataContextWithWebDriverSession(
                    x => x.Screenshots.UseFileNameTemplateWithSessionId());
                var session = context.Sessions.Get<WebDriverSession>();
                session.Go.To(new OrdinaryPage("Test"));

                // Act
                session
                    .Report.Screenshot()
                    .Report.Screenshot("some title");

                // Assert
                VerifyLastLogNestingTextsWithMessagesMatch(
                    minLogLevel: LogLevel.Trace,
                    "^> Take screenshot #1$",
                    $"^< Take screenshot #1 \\(.*\\) >> Artifacts\\.Files\\[\"001-{session.Id}-Test_page.png\"\\]$",
                    "^> Take screenshot #2 some title$",
                    $"^< Take screenshot #2 some title \\(.*\\) >> Artifacts\\.Files\\[\"002-{session.Id}-Test_page-some_title.png\"\\]$");

                CurrentContext.Artifacts.Should.ContainFiles(
                    $"001-{session.Id}-Test_page.png",
                    $"002-{session.Id}-Test_page-some_title.png");
            }

            [Test]
            public void ScreenshotCounterResetsWhenSessionIsPassedToAnotherContext()
            {
                using (var parentContext = ConfigureAtataContextWithWebDriverSession(
                    x => x.UseAsShared())
                    .Build())
                {
                    var session = parentContext.Sessions.Get<WebDriverSession>();
                    session.Go.To(new OrdinaryPage("Test"));

                    using (var childContext1 = ConfigureSessionlessAtataContext()
                        .UseParentContext(parentContext)
                        .Sessions.Borrow<WebDriverSession>()
                        .Build())
                    {
                        childContext1.Sessions.Get<WebDriverSession>().Report.Screenshot("First screenshot");

                        VerifyLastLogNestingTextsWithMessagesMatch(
                            minLogLevel: LogLevel.Trace,
                            "^> Take screenshot #1 First screenshot$",
                            $"^< Take screenshot #1 First screenshot \\(.*\\) >> Artifacts\\.Files\\[\"001-Test_page-First_screenshot.png\"\\]$");

                        CurrentContext.Artifacts.Should.ContainFiles(
                            $"001-Test_page-First_screenshot.png");
                    }

                    using (var childContext2 = ConfigureSessionlessAtataContext()
                        .UseParentContext(parentContext)
                        .Sessions.Borrow<WebDriverSession>()
                        .Build())
                    {
                        childContext2.Sessions.Get<WebDriverSession>().Report.Screenshot("Second screenshot");

                        VerifyLastLogNestingTextsWithMessagesMatch(
                            minLogLevel: LogLevel.Trace,
                            "^> Take screenshot #1 Second screenshot$",
                            $"^< Take screenshot #1 Second screenshot \\(.*\\) >> Artifacts\\.Files\\[\"001-Test_page-Second_screenshot.png\"\\]$");

                        CurrentContext.Artifacts.Should.ContainFiles(
                            $"001-Test_page-Second_screenshot.png");
                    }
                }
            }
        }
    }
}
