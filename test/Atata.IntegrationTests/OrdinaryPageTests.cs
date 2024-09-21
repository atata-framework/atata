namespace Atata.IntegrationTests;

public class OrdinaryPageTests : WebDriverSessionTestSuite
{
    [Test]
    public void WithoutName()
    {
        var page = Go.To<OrdinaryPage>(url: "input");

        CurrentLog.LatestRecord.Message.Should().Contain("Go to \"<ordinary>\" page");

        page.PageTitle.Should.StartWith("Input");
    }

    [Test]
    public void WithName()
    {
        Go.To(new OrdinaryPage("Custom name"), url: "input");

        CurrentLog.LatestRecord.Message.Should().Contain("Go to \"Custom name\" page");
    }
}
