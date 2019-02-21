using FluentAssertions;
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
        public void UIComponent_IsPresent()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.Within(0))
                page.VisibleDiv.IsPresent.Value.Should().BeTrue();

            using (StopwatchAsserter.Within(0))
                page.VisibleDiv.IsPresent.Should.BeTrue();

            using (StopwatchAsserter.Within(0))
                page.VisibleDiv.Should.Exist();

            using (StopwatchAsserter.Within(0))
                page.HiddenDiv.IsPresent.Value.Should().BeTrue();

            using (StopwatchAsserter.Within(0))
                page.HiddenDiv.IsPresent.Should.BeTrue();

            using (StopwatchAsserter.Within(0))
                page.HiddenDiv.Should.Exist();

            using (StopwatchAsserter.Within(0))
                page.HiddenDivWithVisibleVisibility.IsPresent.Value.Should().BeFalse();

            using (StopwatchAsserter.Within(0))
                page.HiddenDivWithVisibleVisibility.IsPresent.Should.BeFalse();

            using (StopwatchAsserter.Within(0))
                page.HiddenDivWithVisibleVisibility.Should.Not.Exist();
        }

        [Test]
        public void UIComponent_IsPresent_Should_Fail()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.Within(2))
                Assert.Throws<AssertionException>(() =>
                    page.VisibleDiv.IsPresent.Should.Within(2).BeFalse());

            using (StopwatchAsserter.Within(2))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDiv.IsPresent.Should.Within(2).BeFalse());

            using (StopwatchAsserter.Within(2))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDivWithVisibleVisibility.IsPresent.Should.Within(2).BeTrue());
        }

        [Test]
        public void UIComponent_IsVisible()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.Within(0))
                page.VisibleDiv.IsVisible.Value.Should().BeTrue();

            using (StopwatchAsserter.Within(0))
                page.VisibleDiv.IsVisible.Should.BeTrue();

            using (StopwatchAsserter.Within(0))
                page.VisibleDiv.Should.BeVisible();

            using (StopwatchAsserter.Within(0))
                page.HiddenDiv.IsVisible.Value.Should().BeFalse();

            using (StopwatchAsserter.Within(0))
                page.HiddenDiv.IsVisible.Should.BeFalse();

            using (StopwatchAsserter.Within(0))
                page.HiddenDiv.Should.BeHidden();

            using (StopwatchAsserter.Within(0))
                page.HiddenDivWithVisibleVisibility.IsVisible.Value.Should().BeFalse();

            using (StopwatchAsserter.Within(0))
                page.HiddenDivWithVisibleVisibility.IsVisible.Should.BeFalse();

            using (StopwatchAsserter.Within(0))
                page.HiddenDivWithVisibleVisibility.Should.BeHidden();
        }

        [Test]
        public void UIComponent_IsVisible_Should_Fail()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.Within(2))
                Assert.Throws<AssertionException>(() =>
                    page.VisibleDiv.IsVisible.Should.Within(2).BeFalse());

            using (StopwatchAsserter.Within(2))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDiv.IsVisible.Should.Within(2).BeTrue());

            using (StopwatchAsserter.Within(2))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDivWithVisibleVisibility.IsVisible.Should.Within(2).BeTrue());
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
