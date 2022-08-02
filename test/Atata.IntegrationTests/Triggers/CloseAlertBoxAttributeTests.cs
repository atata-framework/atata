namespace Atata.IntegrationTests.Triggers;

public class CloseAlertBoxAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        Go.To<MessageBoxPage>()
            .AlertButton.Click();

        Assert.Throws<NoAlertPresentException>(() =>
            _ = AtataContext.Current.Driver.SwitchTo().Alert());
    }
}
