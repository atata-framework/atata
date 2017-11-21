using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class UIComponentTests : UITestFixture
    {
        [Test]
        public void UIComponent_ComponentLocation()
        {
            Go.To<InputPage>().
                TextInput.ComponentLocation.X.Should.BeGreater(10).
                TextInput.ComponentLocation.Y.Should.BeInRange(10, 1000).
                TextInput.ComponentLocation.Y.Get(out int y).
                TextInput.ComponentLocation.Y.Should.Equal(y);
        }

        [Test]
        public void UIComponent_ComponentSize()
        {
            Go.To<InputPage>().
                TextInput.ComponentSize.Width.Should.BeGreater(20).
                TextInput.ComponentSize.Height.Should.BeInRange(10, 100).
                TextInput.ComponentSize.Height.Get(out int height).
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

        [Test]
        public void UIComponent_Wait_Until_Visible()
        {
            Go.To<WaitingOnInitPage>().
                ContentBlock.Wait(Until.Visible).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_Hidden()
        {
            Go.To<WaitingOnInitPage>().
                LoadingBlock.Wait(Until.Hidden).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_MissingOrHidden()
        {
            Go.To<WaitingOnInitPage>().
                LoadingBlock.Wait(Until.MissingOrHidden).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_VisibleThenHidden()
        {
            Go.To<WaitingOnInitPage>().
                LoadingBlock.Wait(Until.VisibleThenHidden).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_HiddenThenVisible()
        {
            Go.To<WaitingOnInitPage>().
                ContentBlock.Wait(Until.HiddenThenVisible).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_WithTimeout()
        {
            var page = Go.To<WaitingOnInitPage>();

            Assert.Throws<NoSuchElementException>(() =>
                page.ContentBlock.Wait(Until.Visible, new WaitOptions(1)));
        }

        [Test]
        public void UIComponent_Wait_WithoutThrow()
        {
            Go.To<WaitingOnInitPage>().
                ContentBlock.Wait(Until.Visible, new WaitOptions(1) { ThrowOnPresenceFailure = false }).
                LoadingBlock.Should.AtOnce.BeVisible();
        }
    }
}
