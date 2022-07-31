namespace Atata.IntegrationTests.Bahaviors;

public class RightClicksUsingScriptAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var block = Go.To<ClickPage>().RightClickBlock;

        block.Metadata.Push(new RightClicksUsingScriptAttribute());

        block.RightClick();

        block.Should.Equal(1);
    }
}
