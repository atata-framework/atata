namespace Atata.IntegrationTests.Bahaviors;

public class DoubleClicksUsingScriptAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var block = Go.To<ClickPage>().DoubleClickBlock;

        block.Metadata.Push(new DoubleClicksUsingScriptAttribute());

        block.DoubleClick();

        block.Should.Equal(1);
    }
}
