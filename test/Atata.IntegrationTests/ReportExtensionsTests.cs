namespace Atata.IntegrationTests;

public class ReportExtensionsTests : UITestFixture
{
    [Test]
    public void Setup()
    {
        AtataContext.Current.Report.Setup(x => x
            .Go.To<OrdinaryPage>());

        VerifyLastLogMessagesMatch(
            minLogLevel: LogLevel.Trace,
            "^> Set up \"<ordinary>\" page$",
            "^> Go to \"<ordinary>\" page",
            "^< Go to \"<ordinary>\" page",
            "^< Set up \"<ordinary>\" page");
    }
}
