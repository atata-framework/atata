namespace Atata.IntegrationTests.Controls;

public class HierarchicalOrderedListTests : UITestFixture
{
    private ListPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<ListPage>();

    [Test]
    public void OfCustomTItemType_WithVisibleVisibilityOfItem()
    {
        var sut = _page.ComplexHierarchicalOrderedList;

        sut.Children.Count.Should.Equal(2);
        sut.Children[0].Content.Should.StartWith("Item 1");
        sut.Children[0].Name.Should.Equal("Item 1");
        sut[1].Name.Should.Equal("Item 2");
        sut.Children.SelectData(x => x.Name).Should.EqualSequence("Item 1", "Item 2");

        sut.Descendants.Count.Should.Equal(8);
        sut.Descendants[0].Name.Should.Equal("Item 1");
        sut.Descendants[1].Name.Should.Equal("Item 1.1");
        sut.Descendants[6].Name.Should.Equal("Item 2.1.2");
        sut.Descendants.SelectData(x => x.Name).Should.Contain("Item 2", "Item 2.1.2", "Item 1.2", "Item 2.1");

        sut.Descendants[x => x.Name == "Item 2.1.1"].Should.Exist();
        sut.Descendants[x => x.Name == "missing"].Should.Not.Exist();
        sut.Descendants.Should.Contain(x => x.Name == "Item 2.1.1");

        sut.Descendants[6].Parent.Name.Should.Equal("Item 2.1");
        sut.Descendants[6].Parent.Parent.Name.Should.Equal("Item 2");
        sut.Descendants[6].Parent.Parent.Parent.Should.Not.Exist();

        sut[0].Children.Count.Should.Equal(2);
        sut[1].Children.Count.Should.Equal(2);
        sut[1][0].Children.Count.Should.Equal(2);
        sut[1][1].Children.Count.Should.Equal(0);
        sut[1][0].Name.Should.Equal("Item 2.1");
        sut[1][0][1].Name.Should.Equal("Item 2.1.2");
        sut[1][0][1].Parent.Parent[0][0].Name.Should.Equal("Item 2.1.1");

        sut[1][0].Number.Should.Equal(2.1m);
        sut.Children.SelectData(x => x.Number).Should.EqualSequence(1, 2);
        sut[1][0][1].Parent.Parent[0][0].Number.Should.Equal(2.11m);

        sut.Children.IndexOf(x => x.Number == 2).Should.Equal(1);
        sut.Descendants.IndexOf(x => x.Number == 2.12m).Should.Equal(6);
    }

    [Test]
    public void OfCustomTItemType_WithAnyVisibilityOfItem()
    {
        var sut = _page.ComplexHierarchicalOrderedListWithAnyVisibilityUsingControlDefinition;

        sut.Children.Count.Should.Equal(3);
        sut.Children[2].Content.Should.StartWith("Item 3");
        sut[2].Name.Should.Equal("Item 3");
        sut.Children.Select(x => x.Name).Should.EqualSequence("Item 1", "Item 2", "Item 3");

        sut.Descendants.Count.Should.Equal(11);
    }
}
