using NUnit.Framework;

namespace Atata.Tests
{
    public class ListTests : UITestFixture
    {
        private ListPage page;

        protected override void OnSetUp()
        {
            page = Go.To<ListPage>();
        }

        [Test]
        public void UnorderedList_Simple()
        {
            var list = page.SimpleUnorderedList;

            list.Items.Count.Should.Equal(3);
            list.Items[0].Content.Should.Equal("Phone 5%");
            list[1].Content.Should.Equal("Book 10%");
            list[2].Content.Should.Equal("Table 15%");
            list.Items.Contents.Should.BeEquivalent("Phone 5%", "Book 10%", "Table 15%");
        }

        [Test]
        public void UnorderedList_Complex()
        {
            var list = page.ComplexUnorderedList;

            list.Items.Count.Should.Equal(3);
            list.Items[0].Name.Should.Equal("Phone");
            list.Items[0].Percent.Should.Equal(0.05m);
            list.Items[2].Name.Should.Equal("Table");
            list.Items[2].Percent.Should.Equal(0.15m);
            list.Items[1].Content.Should.Equal("Book 10%");
        }

        [Test]
        public void UnorderedList_Hierarchical()
        {
            var list = page.UnorderedListForHierarchy;

            list.Items.Count.Should.Equal(2);
        }

        [Test]
        public void OrderedList_Simple()
        {
            var list = page.SimpleOrderedList;

            list.Items.Count.Should.Equal(3);
            list.Items[0].Content.Should.Equal("Phone 20");
            list.Items[1].Content.Should.Equal("Book 30");
            list.Items[2].Content.Should.Equal("Table 40");
            list.Items.Contents.Should.BeEquivalent("Phone 20", "Book 30", "Table 40");
        }

        [Test]
        public void OrderedList_Complex()
        {
            var list = page.ComplexOrderedList;

            list.Items.Count.Should.Equal(3);
            list.Items[0].Name.Should.Equal("Phone");
            list.Items[0].Amount.Should.Equal(20);
            list.Items[2].Name.Should.Equal("Table");
            list.Items[2].Amount.Should.Equal(40);
            list.Items[1].Content.Should.Equal("Book 30");
        }

        [Test]
        public void HierarchicalUnorderedList_Plain()
        {
            var list = page.PlainHierarchicalUnorderedList;

            list.Children.Count.Should.Equal(3);
            list.Children[0].Content.Should.Equal("Phone 5%");
            list[1].Content.Should.Equal("Book 10%");
            list[2].Content.Should.Equal("Table 15%");
            list.Children.Contents.Should.BeEquivalent("Phone 5%", "Book 10%", "Table 15%");

            list.Descendants.Count.Should.Equal(3);
            list.Descendants[0].Content.Should.Equal("Phone 5%");
            list.Descendants.Contents.Should.EqualSequence("Phone 5%", "Book 10%", "Table 15%");

            list[1].Parent.Should.Not.Exist();
        }

        [Test]
        public void HierarchicalUnorderedList_Simple()
        {
            var list = page.SimpleHierarchicalUnorderedList;

            list.Children.Count.Should.Equal(2);
            list.Children[0].Content.Should.StartWith("Item 1");
            list[1].Content.Should.StartWith("Item 2");
            list.Children.Contents.Should.Contain(TermMatch.Contains, "Item 1", "Item 2");

            list.Descendants.Count.Should.Equal(8);
            list.Descendants[0].Content.Should.StartWith("Item 1");
            list.Descendants[1].Content.Should.Equal("Item 1.1");
            list.Descendants[6].Content.Should.Equal("Item 2.1.2");
            list.Descendants.Contents.Should.Contain(TermMatch.Contains, "Item 2", "Item 2.1.2", "Item 1.2", "Item 2.1");

            list.Descendants[x => x.Content == "Item 2.1.1"].Should.Exist();
            list.Descendants[x => x.Content == "missing"].Should.Not.Exist();
            list.Descendants.Contents.Should.Contain("Item 2.1.1");

            list.Descendants[6].Parent.Should.Exist();
            list.Descendants[6].Parent.Parent.Should.Exist();
            list.Descendants[6].Parent.Parent.Parent.Should.Not.Exist();

            list[0].Children.Count.Should.Equal(2);
            list[1].Children.Count.Should.Equal(2);
            list[1][0].Children.Count.Should.Equal(2);
            list[1][1].Children.Count.Should.Equal(0);
            list[1][0][1].Content.Should.Equal("Item 2.1.2");
            list[1][0][1].Parent.Parent[0][0].Content.Should.Equal("Item 2.1.1");
        }

        [Test]
        public void HierarchicalUnorderedList_Complex()
        {
            var list = page.ComplexHierarchicalUnorderedList;

            list.Children.Count.Should.Equal(2);
            list.Children[0].Content.Should.StartWith("Item 1");
            list.Children[0].Name.Should.Equal("Item 1");
            list[1].Name.Should.Equal("Item 2");
            list.Children.SelectData(x => x.Name).Should.EqualSequence("Item 1", "Item 2");

            list.Descendants.Count.Should.Equal(8);
            list.Descendants[0].Name.Should.Equal("Item 1");
            list.Descendants[1].Name.Should.Equal("Item 1.1");
            list.Descendants[6].Name.Should.Equal("Item 2.1.2");
            list.Descendants.SelectData(x => x.Name).Should.Contain("Item 2", "Item 2.1.2", "Item 1.2", "Item 2.1");

            list.Descendants[x => x.Name == "Item 2.1.1"].Should.Exist();
            list.Descendants[x => x.Name == "missing"].Should.Not.Exist();
            list.Descendants.Should.Contain(x => x.Name == "Item 2.1.1");

            list.Descendants[6].Parent.Name.Should.Equal("Item 2.1");
            list.Descendants[6].Parent.Parent.Name.Should.Equal("Item 2");
            list.Descendants[6].Parent.Parent.Parent.Should.Not.Exist();

            list[0].Children.Count.Should.Equal(2);
            list[1].Children.Count.Should.Equal(2);
            list[1][0].Children.Count.Should.Equal(2);
            list[1][1].Children.Count.Should.Equal(0);
            list[1][0].Name.Should.Equal("Item 2.1");
            list[1][0][1].Name.Should.Equal("Item 2.1.2");
            list[1][0][1].Parent.Parent[0][0].Name.Should.Equal("Item 2.1.1");

            list.Children.IndexOf(x => x.Name == "Item 2").Should.Equal(1);
            list.Descendants.IndexOf(x => x.Name == "Item 2.1.2").Should.Equal(6);
        }

        [Test]
        public void HierarchicalOrderedList_Complex()
        {
            var list = page.ComplexHierarchicalOrderedList;

            list.Children.Count.Should.Equal(2);
            list.Children[0].Content.Should.StartWith("Item 1");
            list.Children[0].Name.Should.Equal("Item 1");
            list[1].Name.Should.Equal("Item 2");
            list.Children.SelectData(x => x.Name).Should.EqualSequence("Item 1", "Item 2");

            list.Descendants.Count.Should.Equal(8);
            list.Descendants[0].Name.Should.Equal("Item 1");
            list.Descendants[1].Name.Should.Equal("Item 1.1");
            list.Descendants[6].Name.Should.Equal("Item 2.1.2");
            list.Descendants.SelectData(x => x.Name).Should.Contain("Item 2", "Item 2.1.2", "Item 1.2", "Item 2.1");

            list.Descendants[x => x.Name == "Item 2.1.1"].Should.Exist();
            list.Descendants[x => x.Name == "missing"].Should.Not.Exist();
            list.Descendants.Should.Contain(x => x.Name == "Item 2.1.1");

            list.Descendants[6].Parent.Name.Should.Equal("Item 2.1");
            list.Descendants[6].Parent.Parent.Name.Should.Equal("Item 2");
            list.Descendants[6].Parent.Parent.Parent.Should.Not.Exist();

            list[0].Children.Count.Should.Equal(2);
            list[1].Children.Count.Should.Equal(2);
            list[1][0].Children.Count.Should.Equal(2);
            list[1][1].Children.Count.Should.Equal(0);
            list[1][0].Name.Should.Equal("Item 2.1");
            list[1][0][1].Name.Should.Equal("Item 2.1.2");
            list[1][0][1].Parent.Parent[0][0].Name.Should.Equal("Item 2.1.1");

            list[1][0].Number.Should.Equal(2.1m);
            list.Children.SelectData(x => x.Number).Should.EqualSequence(1, 2);
            list[1][0][1].Parent.Parent[0][0].Number.Should.Equal(2.11m);

            list.Children.IndexOf(x => x.Number == 2).Should.Equal(1);
            list.Descendants.IndexOf(x => x.Number == 2.12m).Should.Equal(6);
        }

        ////[Test]
        ////public void HierarchicalOrderedList_WithAnyVisibility()
        ////{
        ////    var list = page.ComplexHierarchicalOrderedListWithAnyVisibilityUsingControlDefinition;

        ////    list.Children.Count.Should.Equal(3);
        ////    list.Children[2].Content.Should.Equal("Item 3");
        ////    list[2].Name.Should.Equal("Item 3");
        ////    list.Children.SelectData(x => x.Name).Should.EqualSequence("Item 1", "Item 2", "Item 3");

        ////    list.Descendants.Count.Should.Equal(11);
        ////}
    }
}
