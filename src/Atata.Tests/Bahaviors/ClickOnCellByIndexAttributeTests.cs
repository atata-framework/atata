using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class ClickOnCellByIndexAttributeTests : UITestFixture
    {
        [Test]
        public void ClickOnCellByIndexAttribute_Execute()
        {
            var row = Go.To<TablePage>()
                .ClickableCellsTable.Rows[1];

            row.Click();

            AtataContext.Current.AggregateAssert(() =>
            {
                row.Cell1.Should.Equal(0);
                row.Cell2.Should.Equal(0);
                row.Cell3.Should.Equal(1);
            });

            row.Metadata.Push(new ClickOnCellByIndexAttribute(0));
            row.Click();

            AtataContext.Current.AggregateAssert(() =>
            {
                row.Cell1.Should.Equal(1);
                row.Cell2.Should.Equal(0);
                row.Cell3.Should.Equal(1);
            });
        }
    }
}
