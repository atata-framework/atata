﻿namespace Atata.IntegrationTests.Bahaviors;

public class ClicksOnCellByIndexAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var row = Go.To<ClickPage>()
            .ClickableCellsTable.Rows[1];

        row.Metadata.Push(new ClicksOnCellByIndexAttribute(2));

        row.Click();

        CurrentSession.AggregateAssert(() =>
        {
            row.Cells[0].Should.Be(0);
            row.Cells[1].Should.Be(0);
            row.Cells[2].Should.Be(1);
        });

        row.Metadata.Push(new ClicksOnCellByIndexAttribute(0));
        row.Click();

        CurrentSession.AggregateAssert(() =>
        {
            row.Cells[0].Should.Be(1);
            row.Cells[1].Should.Be(0);
            row.Cells[2].Should.Be(1);
        });
    }
}
