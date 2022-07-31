using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class ScrollTriggerTests : UITestFixture
    {
        private ScrollablePage _page;

        protected override bool ReuseDriver => false;

        protected override void OnSetUp()
        {
            _page = Go.To<ScrollablePage>();
        }

        [Test]
        public void ScrollDownAttribute()
        {
            _page.
                BottomText.Should.Not.BeVisibleInViewPort().
                BottomText.Metadata.Add(new ScrollDownAttribute(TriggerEvents.BeforeGet));

            _page.
                BottomText.Get(out _).
                BottomText.Should.BeVisibleInViewPort();
        }

        [Test]
        public void ScrollUpAttribute()
        {
            _page.
                ScrollDown().
                TopText.Should.Not.BeVisibleInViewPort().
                TopText.Metadata.Add(new ScrollUpAttribute(TriggerEvents.BeforeGet));

            _page.
                TopText.Get(out _).
                TopText.Should.BeVisibleInViewPort();
        }

        [Test]
        public void ScrollToAttribute()
        {
            _page.
                BottomText.Should.Not.BeVisibleInViewPort().
                BottomText.Metadata.Add(new ScrollToAttribute(TriggerEvents.BeforeGet));

            _page.
                BottomText.Get(out _).
                BottomText.Should.BeVisibleInViewPort();
        }
    }
}
