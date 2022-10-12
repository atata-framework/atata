namespace Atata.IntegrationTests.Controls;

public class LabelTests : UITestFixture
{
    private LabelPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<LabelPage>();

    [Test]
    public void Value() =>
        _page.FirstNameLabel.Should.Equal("First Name");

    [Test]
    public void For() =>
        _page.FirstNameLabel.For.Should.Equal("first-name");

    [Test]
    public void WithFindByAttributeAttribute() =>
        _page.LastNameByForLabel.Should.Equal("Last Name*");

    [Test]
    public void WithFormatAttribute() =>
        _page.LastNameLabel.Should.Equal("Last Name");
}
