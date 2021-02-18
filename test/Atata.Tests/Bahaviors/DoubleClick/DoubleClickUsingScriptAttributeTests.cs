using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class DoubleClickUsingScriptAttributeTests : UITestFixture
    {
        [Test]
        public void DoubleClickUsingScriptAttribute_Execute()
        {
            var block = Go.To<ClickPage>().DoubleClickBlock;

            block.Metadata.Push(new DoubleClickUsingScriptAttribute());

            block.DoubleClick();

            block.Should.Equal(1);
        }
    }
}
