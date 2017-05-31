using NUnit.Framework;

namespace Atata.Tests
{
    public class UIComponentTest : AutoTest
    {
        [Test]
        public void UIComponent_ComponentSize()
        {
            int height;

            Go.To<InputPage>().
                TextInput.ComponentSize.Width.Should.BeGreater(20).
                TextInput.ComponentSize.Height.Should.BeInRange(10, 100).
                TextInput.ComponentSize.Height.Get(out height).
                TextInput.ComponentSize.Height.Should.Equal(height);
        }
    }
}
