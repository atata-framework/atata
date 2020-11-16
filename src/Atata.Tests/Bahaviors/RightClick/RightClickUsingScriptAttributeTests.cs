using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class RightClickUsingScriptAttributeTests : UITestFixture
    {
        [Test]
        public void RightClickUsingScriptAttribute_Execute()
        {
            var block = Go.To<ClickPage>().RightClickBlock;

            block.Metadata.Push(new RightClickUsingScriptAttribute());

            block.RightClick();

            block.Should.Equal(1);
        }
    }
}
