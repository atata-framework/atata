using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class RightClickUsingActionsAttributeTests : UITestFixture
    {
        [Test]
        public void RightClickUsingActionsAttribute_Execute()
        {
            var block = Go.To<ClickPage>().RightClickBlock;

            block.Metadata.Push(new RightClickUsingActionsAttribute());

            block.RightClick();

            block.Should.Equal(1);
        }
    }
}
