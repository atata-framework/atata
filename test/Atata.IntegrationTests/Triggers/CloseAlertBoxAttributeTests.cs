namespace Atata.IntegrationTests.Triggers;

public class CloseAlertBoxAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<PopupBoxPage>().AlertButton;
        sut.Metadata.Push(new CloseAlertBoxAttribute());

        sut.Click();

        Assert.Throws<NoAlertPresentException>(() =>
            _ = AtataContext.Current.Driver.SwitchTo().Alert());
    }
}
