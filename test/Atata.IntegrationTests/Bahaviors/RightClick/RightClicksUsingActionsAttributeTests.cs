namespace Atata.IntegrationTests.Bahaviors;

public class RightClicksUsingActionsAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var block = Go.To<ClickPage>().RightClickBlock;

        block.Metadata.Push(new RightClicksUsingActionsAttribute());

        block.RightClick();

        block.Should.Be(1);
    }
}
