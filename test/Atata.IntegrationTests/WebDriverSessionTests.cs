namespace Atata.IntegrationTests;

public class WebDriverSessionTests : WebDriverSessionTestSuite
{
    protected override bool ReuseDriver => false;

    [Test]
    public void RestartDriver()
    {
        var session = CurrentSession;
        session.RestartDriver();

        Go.To<BasicControlsPage>();
        Assert.That(session.Driver.Title, Is.Not.Null.Or.Empty);

        session.RestartDriver();

        Assert.That(session.Driver.Title, Is.Null.Or.Empty);
        Go.To<BasicControlsPage>();
    }
}
