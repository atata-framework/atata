using NUnit.Framework;

namespace Atata.IntegrationTests.Bahaviors
{
    public class DoubleClicksUsingScriptAttributeTests : UITestFixture
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
}
