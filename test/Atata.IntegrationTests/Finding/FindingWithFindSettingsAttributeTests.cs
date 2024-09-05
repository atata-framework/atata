namespace Atata.IntegrationTests.Finding;

public class FindingWithFindSettingsAttributeTests : WebDriverSessionTestSuite
{
    private FindingWithSettingsPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<FindingWithSettingsPage>();

    [Test]
    public void WhenAtPageObject() =>
        _page
            .OptionA.Should.Not.BePresent()
            .OptionB.Should.BePresent()
            .OptionC.Should.BePresent()
            .OptionD.Should.Not.BePresent();

    [Test]
    public void WhenAtParentControl() =>
        _page
            .RadioSet.OptionA.Should.Not.BePresent()
            .RadioSet.OptionB.Should.BePresent()
            .RadioSet.OptionC.Should.Not.BePresent()
            .RadioSet.OptionD.Should.BePresent();
}
