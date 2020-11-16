using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class DoubleClickUsingMoveToElementAndDoubleClickActionsAttributeTests : UITestFixture
    {
        [Test]
        public void DoubleClickUsingMoveToElementAndDoubleClickActionsAttribute_Execute()
        {
            var table = Go.To<ClickPage>().ClickableCellsTable;

            table.Metadata.Push(new DoubleClickUsingMoveToElementAndDoubleClickActionsAttribute
            {
                OffsetX = 33,
                OffsetY = 33
            });

            table.DoubleClick();

            table.Rows[2].Cells[2].Should.Equal(2);
        }
    }
}
