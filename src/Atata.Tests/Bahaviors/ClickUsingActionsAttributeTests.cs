using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class ClickUsingActionsAttributeTests : UITestFixture
    {
        private Table<TablePage.ClickableCellsTableRow, TablePage> table;

        protected override void OnSetUp()
        {
            table = Go.To<TablePage>()
                .RefreshPage()
                .ClickableCellsTable;
        }

        [Test]
        public void ClickUsingActionsAttribute_Execute_WithOffsetKind_FromTopLeftInPercents()
        {
            table.Metadata.Push(new ClickUsingActionsAttribute
            {
                OffsetKind = UIComponentOffsetKind.FromTopLeftInPercents,
                OffsetX = 99,
                OffsetY = 99
            });

            table.Click();
            table.Rows[2].Cell3.Should.Equal(1);
        }

        [Test]
        public void ClickUsingActionsAttribute_Execute_WithOffsetKind_FromTopLeftInPixels()
        {
            table.Metadata.Push(new ClickUsingActionsAttribute
            {
                OffsetKind = UIComponentOffsetKind.FromTopLeftInPixels,
                OffsetX = 5,
                OffsetY = 5
            });

            table.Click();
            table.Rows[0].Cell1.Should.Equal(1);
        }

        [Test]
        public void ClickUsingActionsAttribute_Execute_WithOffsetKind_FromCenterInPercents()
        {
            table.Metadata.Push(new ClickUsingActionsAttribute
            {
                OffsetKind = UIComponentOffsetKind.FromCenterInPercents,
                OffsetX = 0,
                OffsetY = -49
            });

            table.Click();
            table.Rows[0].Cell2.Should.Equal(1);
        }

        [Test]
        public void ClickUsingActionsAttribute_Execute_WithOffsetKind_FromCenterInPixels()
        {
            table.Metadata.Push(new ClickUsingActionsAttribute
            {
                OffsetKind = UIComponentOffsetKind.FromCenterInPixels,
                OffsetX = table.ComponentSize.Width / 3,
                OffsetY = table.ComponentSize.Height / 3
            });

            table.Click();
            table.Rows[2].Cell3.Should.Equal(1);
        }
    }
}
