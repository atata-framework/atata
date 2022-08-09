namespace Atata.IntegrationTests.Finding;

public class FindingWithFindSettingsAttributeTests : UITestFixture
{
    private FindingWithSettingsPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<FindingWithSettingsPage>();

    [Test]
    public void WhenAtPageObject() =>
        _page
            .OptionA.Should.Not.Exist()
            .OptionB.Should.Exist()
            .OptionC.Should.Exist()
            .OptionD.Should.Not.Exist();

    [Test]
    public void WhenAtParentControl() =>
        _page
            .RadioSet.OptionA.Should.Not.Exist()
            .RadioSet.OptionB.Should.Exist()
            .RadioSet.OptionC.Should.Not.Exist()
            .RadioSet.OptionD.Should.Exist();
}
