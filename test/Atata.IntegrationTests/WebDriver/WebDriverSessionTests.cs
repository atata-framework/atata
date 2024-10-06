namespace Atata.IntegrationTests.WebDriver;

public static partial class WebDriverSessionTests
{
    public sealed class Report : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void Screenshot()
        {
            var context = BuildAtataContextWithWebDriverSession();
            Go.To<InputPage>();

            context.GetWebDriverSession().Report.Screenshot();

            context.Artifacts.Should.ContainFile("01 Input page.png");
        }

        [Test]
        public void PageSnapshot()
        {
            var context = BuildAtataContextWithWebDriverSession();
            Go.To<InputPage>();

            context.GetWebDriverSession().Report.PageSnapshot();

            context.Artifacts.Should.ContainFile("01 Input page.mhtml");
        }
    }
}
