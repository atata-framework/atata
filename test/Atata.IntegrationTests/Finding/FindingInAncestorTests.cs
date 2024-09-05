namespace Atata.IntegrationTests.Finding;

public class FindingInAncestorTests : WebDriverSessionTestSuite
{
    private FindingInAncestorPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<FindingInAncestorPage>();

    [Test]
    public void First_Visible() =>
        _page.LegendInOneLayer.Should.Equal("Radio Buttons");

    [Test]
    public void ThreeLayers() =>
        _page.LegendInThreeLayers.Should.Equal("Radio Buttons");

    [Test]
    public void ThreeLayers_AtParentAndDeclared() =>
        _page.LegendInThreeLayersAtParentAndDeclared.Should.Equal("Radio Buttons");

    [Test]
    public void ThreeLayers_AtParentAndDeclaredAndComponent() =>
        _page.LegendInThreeLayersAtParentAndDeclaredAndComponent.Should.Equal("Radio Buttons");
}
