namespace Atata.IntegrationTests.Controls;

public class HierarchicalUnorderedListTests : WebDriverSessionTestSuite
{
    private ListPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<ListPage>();

    [Test]
    public void OfHierarchicalListItemType_WhenPlain()
    {
        var sut = _page.PlainHierarchicalUnorderedList;

        sut.Children.Count.Should.Be(3);
        sut.Children[0].Content.Should.Be("Phone 5%");
        sut[1].Content.Should.Be("Book 10%");
        sut[2].Content.Should.Be("Table 15%");
        sut.Children.Contents.Should.BeEquivalent("Phone 5%", "Book 10%", "Table 15%");

        sut.Descendants.Count.Should.Be(3);
        sut.Descendants[0].Content.Should.Be("Phone 5%");
        sut.Descendants.Contents.Should.EqualSequence("Phone 5%", "Book 10%", "Table 15%");

        sut[1].Parent.Should.Not.BePresent();
    }

    [Test]
    public void OfHierarchicalListItemType()
    {
        var sut = _page.SimpleHierarchicalUnorderedList;

        sut.Children.Count.Should.Be(2);
        sut.Children[0].Content.Should.StartWith("Item 1");
        sut[1].Content.Should.StartWith("Item 2");
        sut.Children.Contents.Should.Contain(TermMatch.Contains, "Item 1", "Item 2");

        sut.Descendants.Count.Should.Be(8);
        sut.Descendants[0].Content.Should.StartWith("Item 1");
        sut.Descendants[1].Content.Should.Be("Item 1.1");
        sut.Descendants[6].Content.Should.Be("Item 2.1.2");
        sut.Descendants.Contents.Should.Contain(TermMatch.Contains, "Item 2", "Item 2.1.2", "Item 1.2", "Item 2.1");

        sut.Descendants[x => x.Content == "Item 2.1.1"].Should.BePresent();
        sut.Descendants[x => x.Content == "missing"].Should.Not.BePresent();
        sut.Descendants.Contents.Should.Contain("Item 2.1.1");

        sut.Descendants[6].Parent.Should.BePresent();
        sut.Descendants[6].Parent.Parent.Should.BePresent();
        sut.Descendants[6].Parent.Parent.Parent.Should.Not.BePresent();

        sut[0].Children.Count.Should.Be(2);
        sut[1].Children.Count.Should.Be(2);
        sut[1][0].Children.Count.Should.Be(2);
        sut[1][1].Children.Count.Should.Be(0);
        sut[1][0][1].Content.Should.Be("Item 2.1.2");
        sut[1][0][1].Parent.Parent[0][0].Content.Should.Be("Item 2.1.1");
    }

    [Test]
    public void OfCustomTItemType()
    {
        var sut = _page.ComplexHierarchicalUnorderedList;

        sut.Children.Count.Should.Be(2);
        sut.Children[0].Content.Should.StartWith("Item 1");
        sut.Children[0].Name.Should.Be("Item 1");
        sut[1].Name.Should.Be("Item 2");
        sut.Children.SelectData(x => x.Name).Should.EqualSequence("Item 1", "Item 2");

        sut.Descendants.Count.Should.Be(8);
        sut.Descendants[0].Name.Should.Be("Item 1");
        sut.Descendants[1].Name.Should.Be("Item 1.1");
        sut.Descendants[6].Name.Should.Be("Item 2.1.2");
        sut.Descendants.SelectData(x => x.Name).Should.Contain("Item 2", "Item 2.1.2", "Item 1.2", "Item 2.1");

        sut.Descendants[x => x.Name == "Item 2.1.1"].Should.BePresent();
        sut.Descendants[x => x.Name == "missing"].Should.Not.BePresent();
        sut.Descendants.Should.Contain(x => x.Name == "Item 2.1.1");

        sut.Descendants[6].Parent.Name.Should.Be("Item 2.1");
        sut.Descendants[6].Parent.Parent.Name.Should.Be("Item 2");
        sut.Descendants[6].Parent.Parent.Parent.Should.Not.BePresent();

        sut[0].Children.Count.Should.Be(2);
        sut[1].Children.Count.Should.Be(2);
        sut[1][0].Children.Count.Should.Be(2);
        sut[1][1].Children.Count.Should.Be(0);
        sut[1][0].Name.Should.Be("Item 2.1");
        sut[1][0][1].Name.Should.Be("Item 2.1.2");
        sut[1][0][1].Parent.Parent[0][0].Name.Should.Be("Item 2.1.1");

        sut.Children.IndexOf(x => x.Name == "Item 2").Should.Be(1);
        sut.Descendants.IndexOf(x => x.Name == "Item 2.1.2").Should.Be(6);
    }
}
