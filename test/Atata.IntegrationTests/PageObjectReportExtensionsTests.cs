namespace Atata.IntegrationTests;

public sealed class PageObjectReportExtensionsTests : WebDriverSessionTestSuite
{
    [Test]
    public void Setup()
    {
        CurrentContext.Report.Setup(x => x
            .GetWebDriverSession().Go.To<OrdinaryPage>());

        VerifyLastLogNestingTextsWithMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> Set up \"<ordinary>\" page$",
            "^- > Go to \"<ordinary>\" page",
            "^- < Go to \"<ordinary>\" page",
            "^< Set up \"<ordinary>\" page");
    }
}
