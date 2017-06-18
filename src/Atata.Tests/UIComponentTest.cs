using NUnit.Framework;

namespace Atata.Tests
{
    public class UIComponentTest : AutoTest
    {
        [Test]
        public void UIComponent_ComponentLocation()
        {
            int y;

            Go.To<InputPage>().
                TextInput.ComponentLocation.X.Should.BeGreater(10).
                TextInput.ComponentLocation.Y.Should.BeInRange(10, 1000).
                TextInput.ComponentLocation.Y.Get(out y).
                TextInput.ComponentLocation.Y.Should.Equal(y);
        }

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

        [Test]
        public void UIComponent_Content()
        {
            Go.To<ContentPage>().
                VisibleDiv.Should.Equal("Some text").
                VisibleDiv.Content.Should.Equal("Some text");
        }

        [Test]
        public void UIComponent_Content_Invisible()
        {
            Go.To<ContentPage>().
                HiddenDiv.Should.Not.BeVisible().
                HiddenDiv.Should.BeNull().
                HiddenDiv.Content.Should.BeEmpty().
                HiddenDivUsingTextContent.Should.Equal("Some text").
                HiddenDivUsingTextContent.Content.Should.Equal("Some text").
                HiddenDivUsingInnerHtml.Should.Equal("Some <b>text</b>").
                HiddenDivUsingInnerHtml.Content.Should.Equal("Some <b>text</b>");
        }
    }
}
