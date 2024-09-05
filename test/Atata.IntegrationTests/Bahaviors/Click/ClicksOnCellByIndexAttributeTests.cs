namespace Atata.IntegrationTests.Bahaviors;

public class ClicksOnCellByIndexAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var row = Go.To<ClickPage>()
            .ClickableCellsTable.Rows[1];

        row.Metadata.Push(new ClicksOnCellByIndexAttribute(2));

        row.Click();

        AtataContext.Current.AggregateAssert(() =>
        {
            row.Cells[0].Should.Equal(0);
            row.Cells[1].Should.Equal(0);
            row.Cells[2].Should.Equal(1);
        });

        row.Metadata.Push(new ClicksOnCellByIndexAttribute(0));
        row.Click();

        AtataContext.Current.AggregateAssert(() =>
        {
            row.Cells[0].Should.Equal(1);
            row.Cells[1].Should.Equal(0);
            row.Cells[2].Should.Equal(1);
        });
    }
}
