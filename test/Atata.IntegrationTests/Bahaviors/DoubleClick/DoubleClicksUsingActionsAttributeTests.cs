namespace Atata.IntegrationTests.Bahaviors;

public class DoubleClicksUsingActionsAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var table = Go.To<ClickPage>().ClickableCellsTable;

        table.Metadata.Push(new DoubleClicksUsingActionsAttribute
        {
            OffsetX = 33,
            OffsetY = 33
        });

        table.DoubleClick();

        table.Rows[2].Cells[2].Should.Be(2);
    }
}
