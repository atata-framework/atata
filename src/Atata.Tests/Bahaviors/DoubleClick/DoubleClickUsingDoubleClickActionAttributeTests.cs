using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class DoubleClickUsingDoubleClickActionAttributeTests : UITestFixture
    {
        [Test]
        public void DoubleClickUsingDoubleClickActionAttribute_Execute()
        {
            var table = Go.To<ClickPage>().ClickableCellsTable;

            table.Metadata.Push(new DoubleClickUsingDoubleClickActionAttribute());

            table.DoubleClick();

            table.Rows[1].Cells[1].Should.Equal(2);
        }
    }
}
