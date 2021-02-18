using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class DoubleClickUsingActionsAttributeTests : UITestFixture
    {
        [Test]
        public void DoubleClickUsingActionsAttribute_Execute()
        {
            var table = Go.To<ClickPage>().ClickableCellsTable;

            table.Metadata.Push(new DoubleClickUsingActionsAttribute
            {
                OffsetX = 33,
                OffsetY = 33
            });

            table.DoubleClick();

            table.Rows[2].Cells[2].Should.Equal(2);
        }
    }
}
