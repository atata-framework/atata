namespace Atata.IntegrationTests.Bahaviors;

public class ClicksUsingActionsAttributeTests : UITestFixture
{
    private Table<ClickPage.ClickableCellsTableRow, ClickPage> _table;

    protected override void OnSetUp() =>
        _table = Go.To<ClickPage>()
            .RefreshPage()
            .ClickableCellsTable;

    [Test]
    public void Execute_WithOffsetKind_FromTopLeftInPercents()
    {
        _table.Metadata.Push(new ClicksUsingActionsAttribute
        {
            OffsetKind = UIComponentOffsetKind.FromTopLeftInPercents,
            OffsetX = 99,
            OffsetY = 99
        });

        _table.Click();
        _table.Rows[2].Cells[2].Should.Equal(1);
    }

    [Test]
    public void Execute_WithOffsetKind_FromTopLeftInPixels()
    {
        _table.Metadata.Push(new ClicksUsingActionsAttribute
        {
            OffsetKind = UIComponentOffsetKind.FromTopLeftInPixels,
            OffsetX = 5,
            OffsetY = 5
        });

        _table.Click();
        _table.Rows[0].Cells[0].Should.Equal(1);
    }

    [Test]
    public void Execute_WithOffsetKind_FromCenterInPercents()
    {
        _table.Metadata.Push(new ClicksUsingActionsAttribute
        {
            OffsetKind = UIComponentOffsetKind.FromCenterInPercents,
            OffsetX = 0,
            OffsetY = -49
        });

        _table.Click();
        _table.Rows[0].Cells[1].Should.Equal(1);
    }

    [Test]
    public void Execute_WithOffsetKind_FromCenterInPixels()
    {
        _table.Metadata.Push(new ClicksUsingActionsAttribute
        {
            OffsetKind = UIComponentOffsetKind.FromCenterInPixels,
            OffsetX = _table.ComponentSize.Width / 3,
            OffsetY = _table.ComponentSize.Height / 3
        });

        _table.Click();
        _table.Rows[2].Cells[2].Should.Equal(1);
    }
}
