namespace Atata.IntegrationTests.Bahaviors;

public class RightClicksUsingScriptAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var block = Go.To<ClickPage>().RightClickBlock;

        block.Metadata.Push(new RightClicksUsingScriptAttribute());

        block.RightClick();

        block.Should.Be(1);
    }
}
