namespace Atata.IntegrationTests.Bahaviors;

public class ClicksUsingScriptAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var cell = Go.To<ClickPage>()
            .ClickableCellsTable.Rows[1].Cells[2];

        cell.Metadata.Push(new ClicksUsingScriptAttribute());

        cell.Click();

        cell.Should.Equal(1);
    }
}
