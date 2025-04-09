namespace Atata.IntegrationTests.Controls;

public class LabelTests : WebDriverSessionTestSuite
{
    private LabelPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<LabelPage>();

    [Test]
    public void Value() =>
        _page.FirstNameLabel.Should.Be("First Name");

    [Test]
    public void For() =>
        _page.FirstNameLabel.For.Should.Be("first-name");

    [Test]
    public void WithFindByAttributeAttribute() =>
        _page.LastNameByForLabel.Should.Be("Last Name*");

    [Test]
    public void WithFormatAttribute() =>
        _page.LastNameLabel.Should.Be("Last Name");
}
