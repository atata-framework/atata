using NUnit.Framework;

namespace Atata.Tests
{
    public class ScrollTests : UITestFixture
    {
        private ScrollablePage page;

        protected override bool ReuseDriver => true;

        protected override void OnSetUp()
        {
            page = Go.To<ScrollablePage>().
                Press(OpenQA.Selenium.Keys.Home).
                Wait(System.TimeSpan.FromMilliseconds(200));
        }

        [Test]
        public void ScrollDown_Method()
        {
            page.
                BottomText.Should.Not.BeVisibleInViewPort().
                ScrollDown().
                BottomText.Should.BeVisibleInViewPort();
        }

        [Test]
        public void ScrollUp_Method()
        {
            page.
                BottomText.Click().
                BottomText.Should.BeVisibleInViewPort().
                ScrollUp().
                BottomText.Should.Not.BeVisibleInViewPort();
        }

        [Test]
        public void ScrollDown_Trigger()
        {
            page.
                BottomText.Should.Not.BeVisibleInViewPort().
                BottomText.Hover().
                BottomText.Should.BeVisibleInViewPort();
        }

        [Test]
        public void ScrollUp_Trigger()
        {
            page.
                BottomText.Click().
                BottomText.Should.BeVisibleInViewPort().
                TopText.Hover().
                BottomText.Should.Not.BeVisibleInViewPort();
        }
    }
}
