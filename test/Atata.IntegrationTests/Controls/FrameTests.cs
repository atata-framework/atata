namespace Atata.IntegrationTests.Controls;

[Category("Unstable")]
public class FrameTests : WebDriverSessionTestSuite
{
    private FramePage _page;

    protected override void OnSetUp() =>
        _page = Go.To<FramePage>();

    [Test]
    public void OfTypeWithTFramePageObject() =>
        _page
            .Frame1.SwitchTo()
                .TextInput.Set("abc")
                .SwitchToRoot<FramePage>()
            .Header.Should.Be("Frame")
            .Frame2.SwitchTo()
                .Select.Set(4)
                .SwitchBack()

            .Header.Should.Be("Frame")
            .Frame1.SwitchTo()
                .Header.Should.Be("Frame Inner 1")
                .TextInput.Should.Be("abc")
                .SwitchToRoot<FramePage>()
            .Frame2.SwitchTo()
                .Header.Should.Be("Frame Inner 2")
                .Select.Should.Be(4);

    [Test]
    public void OfTypeWithoutTFramePageObject() =>
        _page
            .Frame1Generic.SwitchTo<FrameInner1Page>()
                .TextInput.Set("abc")
                .SwitchToRoot<FramePage>()
            .Header.Should.Be("Frame")
            .Frame1Generic.SwitchTo<FrameInner1Page>()
                .Header.Should.Be("Frame Inner 1")
                .TextInput.Should.Be("abc");

    [Test]
    public void WithGoTemporarilyAttribute() =>
        _page
            .SetState(1)
            .Frame1Temporarily.SwitchTo()
                .TextInput.Set("abc")
                .SwitchToRoot<FramePage>()
            .State.Should.Be(1)
            .SetState(2)
            .Frame2Temporarily.SwitchTo()
                .SwitchBack()
            .State.Should.Be(2)
            .SetState(3)
            .Frame2.SwitchTo(temporarily: true)
                .SwitchBack()
            .State.Should.Be(3)
            .SetState(4)
            .Frame1Temporarily.SwitchTo(temporarily: false)
                .SwitchToRoot<FramePage>()
            .State.Should.Be(0);

    [Test]
    public void DoWithin() =>
        _page
            .Frame1.DoWithin(x => x
                .TextInput.Set("abc"))
            .Header.Should.Be("Frame")
            .Frame2.DoWithin(x => x
                .Select.Set(4))

            .Header.Should.Be("Frame")
            .Frame1.DoWithin(x => x
                .Header.Should.Be("Frame Inner 1")
                .TextInput.Should.Be("abc"))
            .Frame2.DoWithin(x => x
                .Header.Should.Be("Frame Inner 2")
                .Select.Should.Be(4));

    [Test]
    public void DoWithin_WithGoTemporarilyAttribute() =>
        _page
            .SetState(1)
            .Frame1Temporarily.DoWithin(
                x => x.TextInput.Set("abc"))
            .State.Should.Be(1)
            .SetState(2)
            .Frame1Generic.DoWithin<FrameInner1Page>(
                x => x.TextInput.Should.Be("abc"),
                temporarily: true)
            .State.Should.Be(2)
            .SetState(3)
            .Frame2.DoWithin(
                x => x.Select.Set(2),
                temporarily: true)
            .State.Should.Be(3)
            .SetState(4)
            .Frame1Temporarily.DoWithin(
                x => x.TextInput.Should.Be("abc"),
                temporarily: false)
            .State.Should.Be(0);
}
