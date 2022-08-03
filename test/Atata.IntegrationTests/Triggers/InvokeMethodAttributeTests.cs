namespace Atata.IntegrationTests.Triggers;

public class InvokeMethodAttributeTests : UITestFixture
{
    private TriggersPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<TriggersPage>();

    [Test]
    public void Execute_AtProperty()
    {
        _page.Perform.Click();

        Assert.That(_page.IsBeforePerformInvoked, Is.True);
        Assert.That(_page.IsAfterPerformInvoked, Is.True);
    }

    [Test]
    public void Execute_AtComponent() =>
        Assert.That(TriggersPage.IsOnInitInvoked, Is.True);
}
