namespace Atata.IntegrationTests.Controls;

public class UnorderedListTests : UITestFixture
{
    private ListPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<ListPage>();

    [Test]
    public void OfTypeWithoutTItem()
    {
        var sut = _page.SimpleUnorderedList;

        sut.Items.Count.Should.Equal(3);
        sut.Items[0].Should.Equal("Phone 5%");
        sut[1].Should.Equal("Book 10%");
        sut[2].Should.Equal("Table 15%");
        sut.Items.Should.BeEquivalent("Phone 5%", "Book 10%", "Table 15%");
    }

    [Test]
    public void OfTypeWithTItem()
    {
        var sut = _page.ComplexUnorderedList;

        sut.Items.Count.Should.Equal(3);
        sut.Items[0].Name.Should.Equal("Phone");
        sut.Items[0].Percent.Should.Equal(0.05m);
        sut.Items[2].Name.Should.Equal("Table");
        sut.Items[2].Percent.Should.Equal(0.15m);
        sut.Items[1].Content.Should.Equal("Book 10%");
    }

    [Test]
    public void OfTypeWithTItem_WhichIsHierarchical()
    {
        var sut = _page.UnorderedListForHierarchy;

        sut.Items.Count.Should.Equal(2);
    }
}
