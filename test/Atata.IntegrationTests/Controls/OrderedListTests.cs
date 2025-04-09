namespace Atata.IntegrationTests.Controls;

public class OrderedListTests : WebDriverSessionTestSuite
{
    private ListPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<ListPage>();

    [Test]
    public void OfTypeWithoutTItem()
    {
        var sut = _page.SimpleOrderedList;

        sut.Items.Count.Should.Be(3);
        sut.Items[0].Should.Be("Phone 20");
        sut.Items[1].Should.Be("Book 30");
        sut.Items[2].Should.Be("Table 40");
        sut.Items.Should.BeEquivalent("Phone 20", "Book 30", "Table 40");
    }

    [Test]
    public void OfTypeWithTItem()
    {
        var sut = _page.ComplexOrderedList;

        sut.Items.Count.Should.Be(3);
        sut.Items[0].Name.Should.Be("Phone");
        sut.Items[0].Amount.Should.Be(20);
        sut.Items[2].Name.Should.Be("Table");
        sut.Items[2].Amount.Should.Be(40);
        sut.Items[1].Content.Should.Be("Book 30");
    }
}
