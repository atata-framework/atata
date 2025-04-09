namespace Atata.IntegrationTests.WebDriver;

public static partial class WebDriverSessionTests
{
    public sealed class Report : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void Screenshot()
        {
            var context = BuildAtataContextWithWebDriverSession();
            string sessionId = context.Sessions.Get<WebDriverSession>().Id;
            Go.To<InputPage>();

            context.Sessions.Get<WebDriverSession>().Report.Screenshot();

            context.Artifacts.Should.ContainFile($"{sessionId}-01 Input page.png");
        }

        [Test]
        public void PageSnapshot()
        {
            var context = BuildAtataContextWithWebDriverSession();
            string sessionId = context.Sessions.Get<WebDriverSession>().Id;
            Go.To<InputPage>();

            context.Sessions.Get<WebDriverSession>().Report.PageSnapshot();

            context.Artifacts.Should.ContainFile($"{sessionId}-01 Input page.mhtml");
        }
    }
}
