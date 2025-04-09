namespace Atata.IntegrationTests.Bahaviors;

public class ClicksUsingScriptAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var cell = Go.To<ClickPage>()
            .ClickableCellsTable.Rows[1].Cells[2];

        cell.Metadata.Push(new ClicksUsingScriptAttribute());

        cell.Click();

        cell.Should.Be(1);
    }
}
