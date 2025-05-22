namespace Atata.IntegrationTests.WebDriver;

public partial class WebDriverSessionTests
{
    public sealed class TakePageSnapshot : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();
            session.Go.To<InputPage>();

            var artifact = session.TakePageSnapshot();

            artifact!.Should.Exist()
                .Name.Should.Be("001-Input_page.mhtml");
        }

        [Test]
        public void WithTitle_WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();
            session.Go.To<InputPage>();

            var artifact = session.TakePageSnapshot("Test");

            artifact!.Should.Exist()
                .Name.Should.Be("001-Input_page-Test.mhtml");
        }

        [Test]
        public void WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();

            var artifact = session.TakePageSnapshot();

            artifact!.Should.Exist()
                .Name.Should.Be("001.mhtml");
        }

        [Test]
        public void WithTitle_WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();
            var session = context.Sessions.Get<WebDriverSession>();

            var artifact = session.TakePageSnapshot("Test");

            artifact!.Should.Exist()
                .Name.Should.Be("001-Test.mhtml");
        }

        [Test]
        public void WhenThrows()
        {
            var context = ConfigureAtataContextWithWebDriverSession(
                session => session.PageSnapshots.UseStrategy(Mock.Of<IPageSnapshotStrategy<WebDriverSession>>(MockBehavior.Strict)))
                .Build();
            var session = context.Sessions.Get<WebDriverSession>();
            session.Go.To<InputPage>();

            var artifact = session.TakePageSnapshot();

            VerifyLastLogNestingTextsWithMessagesMatch(LogLevel.Error, "^Page snapshot failed");
            artifact.ToSubject().Should.BeNull();
            context.Artifacts.Should.Not.Exist();
        }
    }
}
