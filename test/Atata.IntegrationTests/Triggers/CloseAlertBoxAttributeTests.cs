namespace Atata.IntegrationTests.Triggers;

public class CloseAlertBoxAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<PopupBoxPage>().AlertButton;
        sut.Metadata.Push(new CloseAlertBoxAttribute());

        sut.Click();

        AssertThatPopupBoxIsNotOpen();
    }
}
