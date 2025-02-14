namespace Atata.IntegrationTests.WebDriver;

public partial class WebDriverSessionTests
{
    public sealed class TakeScreenshot : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.GetWebDriverSession();
            session.Go.To<InputPage>();

            session.TakeScreenshot();

            context.Artifacts.Should.ContainFile($"{session.Id}-01 Input page.png");
        }

        [Test]
        public void WithTitle_WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.GetWebDriverSession();
            session.Go.To<InputPage>();

            session.TakeScreenshot("Test");

            context.Artifacts.Should.ContainFile($"{session.Id}-01 Input page - Test.png");
        }

        [Test]
        public void WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.GetWebDriverSession();

            session.TakeScreenshot();

            context.Artifacts.Should.ContainFile($"{session.Id}-01.png");
        }

        [Test]
        public void WithTitle_WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.GetWebDriverSession();

            session.TakeScreenshot("Test");

            context.Artifacts.Should.ContainFile($"{session.Id}-01 - Test.png");
        }

        [Test]
        public void WhenThrows()
        {
            var context = ConfigureAtataContextWithWebDriverSession(
                session => session.Screenshots.UseStrategy(Mock.Of<IScreenshotStrategy<WebDriverSession>>(MockBehavior.Strict)))
                .Build();
            var session = context.GetWebDriverSession();
            session.Go.To<InputPage>();

            session.TakeScreenshot();

            VerifyLastLogNestingTextsWithMessagesMatch(LogLevel.Error, "Screenshot failed");
            context.Artifacts.Should.Not.Exist();
        }
    }
}
