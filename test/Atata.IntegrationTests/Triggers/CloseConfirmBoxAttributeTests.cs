namespace Atata.IntegrationTests.Triggers;

public class CloseConfirmBoxAttributeTests : UITestFixture
{
    protected override bool ReuseDriver => false;

    [Test]
    public void Execute_WithAcceptTrue()
    {
        var page = Go.To<MessageBoxPage>();

        page.ConfirmButton.Click();

        Assert.Throws<NoAlertPresentException>(() =>
            _ = AtataContext.Current.Driver.SwitchTo().Alert());
        page.PageTitle.Should.StartWith("Go");
    }

    [Test]
    public void Execute_WithAcceptFalse()
    {
        var page = Go.To<MessageBoxPage>();

        page.ConfirmButtonWithReject.Click();

        Assert.Throws<NoAlertPresentException>(() =>
            _ = AtataContext.Current.Driver.SwitchTo().Alert());
        page.PageTitle.Should.StartWith("Message Box");
    }
}
