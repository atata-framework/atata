namespace Atata.IntegrationTests.Triggers;

public class CloseConfirmBoxAttributeTests : WebDriverSessionTestSuite
{
    protected override bool ReuseDriver => false;

    [Test]
    public void Execute_WithAcceptTrue()
    {
        var page = Go.To<PopupBoxPage>();
        var sut = page.ConfirmButton;
        sut.Metadata.Push(new CloseConfirmBoxAttribute());

        sut.Click();

        AssertThatPopupBoxIsNotOpen();
        page.PageTitle.Should.StartWith("Go");
    }

    [Test]
    public void Execute_WithAcceptFalse()
    {
        var page = Go.To<PopupBoxPage>();
        var sut = page.ConfirmButton;
        sut.Metadata.Push(new CloseConfirmBoxAttribute(false));

        sut.Click();

        AssertThatPopupBoxIsNotOpen();
        page.PageTitle.Should.StartWith("Popup Box");
    }
}
