using NUnit.Framework;

namespace Atata.Tests
{
    public class ListTest : AutoTest
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
            list.Items[1].Content.Should.Equal("Book 10%");
            list.Items[2].Content.Should.Equal("Table 15%");
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
            var list = page.HierarchicalUnorderedList;

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
    }
}
