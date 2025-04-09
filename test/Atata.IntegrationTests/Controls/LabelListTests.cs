namespace Atata.IntegrationTests.Controls;

public class LabelListTests : WebDriverSessionTestSuite
{
    private LabelList<LabelPage> _sut;

    protected override void OnSetUp() =>
        _sut = Go.To<LabelPage>().Labels;

    [Test]
    public void FuncIndexer() =>
        _sut[x => x.FirstName].Should.Be("First Name");

    [Test]
    public void FuncIndexer_MissingItem() =>
        _sut[x => x.CompanyName].Should.Not.BePresent();

    [Test]
    public void For() =>
        _sut.For(x => x.LastName).Should.Be("Last Name*");

    [Test]
    public void Count() =>
        _sut.Count.Should.BeGreater(1);
}
