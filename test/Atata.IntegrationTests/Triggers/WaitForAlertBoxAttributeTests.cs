namespace Atata.IntegrationTests.Triggers;

public class WaitForAlertBoxAttributeTests : UITestFixture
{
    protected override bool ReuseDriver => false;

    [Test]
    public void Execute()
    {
        var sut = Go.To<PopupBoxPage>().AlertWithDelayButton;
        sut.Metadata.Push(new WaitForAlertBoxAttribute());

        sut.Click();

        AssertThatPopupBoxIsOpen();
    }

    [Test]
    public void Execute_WithTimeout()
    {
        var sut = Go.To<PopupBoxPage>().NoneButton;
        sut.Metadata.Push(new WaitForAlertBoxAttribute { Timeout = 1 });

        Assert.Throws<TimeoutException>(() =>
            sut.Click());
    }
}
