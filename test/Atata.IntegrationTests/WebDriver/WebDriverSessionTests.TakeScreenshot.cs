namespace Atata.IntegrationTests.WebDriver;

public partial class WebDriverSessionTests
{
    public class TakeScreenshot : WebDriverSessionTestSuiteBase
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
        public void WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();

            context.GetWebDriverSession().TakeScreenshot();

            context.Artifacts.Should.ContainFile("01.png");
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
