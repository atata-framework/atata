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
                BottomText.Should.Not.BeDisplayedWithinViewPort().
                BottomText.Click().
                BottomText.Should.BeDisplayedWithinViewPort();
        }

        [Test]
        public void PressHome_BeforeClick()
        {
            page.
                Press(Keys.End).
                TopText.Should.Not.BeDisplayedWithinViewPort().
                TopText.Click().
                TopText.Should.BeDisplayedWithinViewPort();
        }
    }
}
