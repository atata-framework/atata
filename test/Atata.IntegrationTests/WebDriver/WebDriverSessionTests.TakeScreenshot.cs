namespace Atata.IntegrationTests.WebDriver;

public partial class WebDriverSessionTests
{
    public sealed class TakeScreenshot : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            Go.To<InputPage>();

            context.GetWebDriverSession().TakeScreenshot();

            context.Artifacts.Should.ContainFile("01 Input page.png");
        }

        [Test]
        public void WithTitle_WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            Go.To<InputPage>();

            context.GetWebDriverSession().TakeScreenshot("Test");

            context.Artifacts.Should.ContainFile("01 Input page - Test.png");
        }

        [Test]
        public void WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();

            context.GetWebDriverSession().TakeScreenshot();

            context.Artifacts.Should.ContainFile("01.png");
        }

        [Test]
        public void WithTitle_WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();

            context.GetWebDriverSession().TakeScreenshot("Test");

            context.Artifacts.Should.ContainFile("01 - Test.png");
        }

        [Test]
        public void WhenThrows()
        {
            var context = ConfigureAtataContextWithWebDriverSession(
                session => session.Screenshots.UseStrategy(Mock.Of<IScreenshotStrategy<WebDriverSession>>(MockBehavior.Strict)))
                .Build();
            Go.To<InputPage>();

            context.GetWebDriverSession().TakeScreenshot();

            VerifyLastLogMessagesContain(LogLevel.Error, "Screenshot failed");
            context.Artifacts.Should.Not.Exist();
        }
    }
}
