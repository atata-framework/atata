using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class RightClicksUsingActionsAttributeTests : UITestFixture
    {
        [Test]
        public void Execute()
        {
            var block = Go.To<ClickPage>().RightClickBlock;

            block.Metadata.Push(new RightClicksUsingActionsAttribute());

            block.RightClick();

            block.Should.Equal(1);
        }
    }
}
