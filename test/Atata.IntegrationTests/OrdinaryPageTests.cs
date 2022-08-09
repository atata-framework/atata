namespace Atata.IntegrationTests;

public class OrdinaryPageTests : UITestFixture
{
    [Test]
    public void WithoutName()
    {
        var page = Go.To<OrdinaryPage>(url: "input");

        Assert.That(LogEntries.Last().Message, Is.EqualTo("Go to \"<ordinary>\" page"));

        page.PageTitle.Should.StartWith("Input");
    }

    [Test]
    public void WithName()
    {
        Go.To(new OrdinaryPage("Custom name"), url: "input");

        Assert.That(LogEntries.Last().Message, Is.EqualTo("Go to \"Custom name\" page"));
    }
}
