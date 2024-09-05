namespace Atata.IntegrationTests.Triggers;

public class WaitForScriptAttributeTests : WebDriverSessionTestSuite
{
    private WaitingPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<WaitingPage>();

    [Test]
    public void Execute() =>
        _page
            .ButtonWithSuccessfulScriptWait()
            .ValueBlock.Should.AtOnce.BePresent();

    [Test]
    public void Execute_WithTimeout()
    {
        using (StopwatchAsserter.WithinSeconds(1))
            Assert.Throws<TimeoutException>(
                () => _page.ButtonWithTimeoutScriptWait());
    }
}
