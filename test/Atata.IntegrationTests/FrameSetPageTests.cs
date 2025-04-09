namespace Atata.IntegrationTests;

public class FrameSetPageTests : WebDriverSessionTestSuite
{
    private FrameSetPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<FrameSetPage>();

    [Test]
    public void Interact() =>
        _page
            .Frame1.SwitchTo()
                .Header.Should.Be("Frame Inner 1")
                .TextInput.Set("123")
                .TextInput.Should.Be("123")
                .SwitchToRoot<FrameSetPage>()
            .Frame2.DoWithin(x => x
                .Header.Should.Be("Frame Inner 2")
                .Select.Should.Be(1))
            .Frame3.SwitchTo()
                .Header.Should.Be("Frame Inner 1")
                .TextInput.Should.BeNullOrEmpty();
}
