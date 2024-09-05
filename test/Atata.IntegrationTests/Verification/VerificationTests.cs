namespace Atata.IntegrationTests.Verification;

public class VerificationTests : WebDriverSessionTestSuite
{
    private WaitingPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<WaitingPage>();

    [Test]
    public void Default() =>
        _page.ButtonWithoutWait.Click()
            .Result.Should.BePresent();

    [Test]
    public void AtOnce()
    {
        _page.ButtonWithoutWait.Click();

        AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
            _page.Result.Should.AtOnce.BePresent());
    }
}
