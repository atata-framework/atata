namespace Atata.IntegrationTests.Controls;

[Category("Unstable")]
public class FrameTests : UITestFixture
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
            .Header.Should.Equal("Frame")
            .Frame2.SwitchTo()
                .Select.Set(4)
                .SwitchBack()

            .Header.Should.Equal("Frame")
            .Frame1.SwitchTo()
                .Header.Should.Equal("Frame Inner 1")
                .TextInput.Should.Equal("abc")
                .SwitchToRoot<FramePage>()
            .Frame2.SwitchTo()
                .Header.Should.Equal("Frame Inner 2")
                .Select.Should.Equal(4);

    [Test]
    public void OfTypeWithoutTFramePageObject() =>
        _page
            .Frame1Generic.SwitchTo<FrameInner1Page>()
                .TextInput.Set("abc")
                .SwitchToRoot<FramePage>()
            .Header.Should.Equal("Frame")
            .Frame1Generic.SwitchTo<FrameInner1Page>()
                .Header.Should.Equal("Frame Inner 1")
                .TextInput.Should.Equal("abc");

    [Test]
    public void WithGoTemporarilyAttribute() =>
        _page
            .SetState(1)
            .Frame1Temporarily.SwitchTo()
                .TextInput.Set("abc")
                .SwitchToRoot<FramePage>()
            .State.Should.Equal(1)
            .SetState(2)
            .Frame2Temporarily.SwitchTo()
                .SwitchBack()
            .State.Should.Equal(2)
            .SetState(3)
            .Frame2.SwitchTo(temporarily: true)
                .SwitchBack()
            .State.Should.Equal(3)
            .SetState(4)
            .Frame1Temporarily.SwitchTo(temporarily: false)
                .SwitchToRoot<FramePage>()
            .State.Should.Equal(0);

    [Test]
    public void DoWithin() =>
        _page
            .Frame1.DoWithin(x => x
                .TextInput.Set("abc"))
            .Header.Should.Equal("Frame")
            .Frame2.DoWithin(x => x
                .Select.Set(4))

            .Header.Should.Equal("Frame")
            .Frame1.DoWithin(x => x
                .Header.Should.Equal("Frame Inner 1")
                .TextInput.Should.Equal("abc"))
            .Frame2.DoWithin(x => x
                .Header.Should.Equal("Frame Inner 2")
                .Select.Should.Equal(4));

    [Test]
    public void DoWithin_WithGoTemporarilyAttribute() =>
        _page
            .SetState(1)
            .Frame1Temporarily.DoWithin(
                x => x.TextInput.Set("abc"))
            .State.Should.Equal(1)
            .SetState(2)
            .Frame1Generic.DoWithin<FrameInner1Page>(
                x => x.TextInput.Should.Equal("abc"),
                temporarily: true)
            .State.Should.Equal(2)
            .SetState(3)
            .Frame2.DoWithin(
                x => x.Select.Set(2),
                temporarily: true)
            .State.Should.Equal(3)
            .SetState(4)
            .Frame1Temporarily.DoWithin(
                x => x.TextInput.Should.Equal("abc"),
                temporarily: false)
            .State.Should.Equal(0);
}
