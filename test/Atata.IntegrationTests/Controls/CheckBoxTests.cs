namespace Atata.IntegrationTests.Controls;

public class CheckBoxTests : UITestFixture
{
    private CheckBoxListPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<CheckBoxListPage>();

    [Test]
    public void Interact()
    {
        var sut = _page.OptionA;

        sut.Should.BeFalse();
        sut.Should.Not.BeChecked();
        sut.Set(true);
        sut.Should.BeTrue();
        sut.Should.BeChecked();
        sut.Uncheck();
        sut.Should.Not.BeTrue();
        sut.Should.BeUnchecked();
        sut.Check();
        sut.Should.BeChecked();
    }
}
