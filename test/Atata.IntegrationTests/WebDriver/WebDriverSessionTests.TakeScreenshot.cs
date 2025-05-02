namespace Atata.IntegrationTests.WebDriver;

public partial class WebDriverSessionTests
{
    public sealed class TakeScreenshot : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();
            session.Go.To<InputPage>();

            var artifact = session.TakeScreenshot();

            artifact!.Should.Exist()
                .Name.Should.Be($"{session.Id}-01 Input page.png");
        }

        [Test]
        public void WithTitle_WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();
            session.Go.To<InputPage>();

            var artifact = session.TakeScreenshot("Test");

            artifact!.Should.Exist()
                .Name.Should.Be($"{session.Id}-01 Input page - Test.png");
        }

        [Test]
        public void WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();

            var artifact = session.TakeScreenshot();

            artifact!.Should.Exist()
                .Name.Should.Be($"{session.Id}-01.png");
        }

        [Test]
        public void WithTitle_WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();

            var artifact = session.TakeScreenshot("Test");

            artifact!.Should.Exist()
                .Name.Should.Be($"{session.Id}-01 - Test.png");
        }

        [Test]
        public void WhenThrows()
        {
            var context = ConfigureAtataContextWithWebDriverSession(
                session => session.Screenshots.UseStrategy(Mock.Of<IScreenshotStrategy<WebDriverSession>>(MockBehavior.Strict)))
                .Build();
            var session = context.Sessions.Get<WebDriverSession>();
            session.Go.To<InputPage>();

            var artifact = session.TakeScreenshot();

            VerifyLastLogNestingTextsWithMessagesMatch(LogLevel.Error, "^Screenshot failed");
            artifact.ToSubject().Should.BeNull();
            context.Artifacts.Should.Not.Exist();
        }
    }
}
