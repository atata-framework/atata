using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class PressTriggerTests : UITestFixture
    {
        private PressTriggersPage page;

        protected override void OnSetUp()
        {
            page = Go.To<PressTriggersPage>();
        }

        [Test]
        public void PressEnd_BeforeClick()
        {
            page.
                BottomText.Should.Not.BeVisibleInViewPort().
                BottomText.Click().
                BottomText.Should.BeVisibleInViewPort();
        }

        [Test]
        public void PressHome_BeforeClick()
        {
            page.
                Press(Keys.End).
                TopText.Should.Not.BeVisibleInViewPort().
                TopText.Click().
                TopText.Should.BeVisibleInViewPort();
        }
    }
}
