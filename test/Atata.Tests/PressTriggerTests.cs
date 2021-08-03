using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class PressTriggerTests : UITestFixture
    {
        private ScrollablePage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<ScrollablePage>();
        }

        [Test]
        public void PressEndAttribute()
        {
            _page.
                BottomText.Should.Not.BeVisibleInViewPort().
                BottomText.Metadata.Add(new PressEndAttribute(TriggerEvents.BeforeGet));

            _page.
                BottomText.Get(out _).
                BottomText.Should.BeVisibleInViewPort();
        }

        [Test]
        public void PressHomeAttribute()
        {
            _page.
                Press(Keys.End).
                TopText.Should.Not.BeVisibleInViewPort().
                TopText.Metadata.Add(new PressHomeAttribute(TriggerEvents.BeforeGet));

            _page.
                TopText.Get(out _).
                TopText.Should.BeVisibleInViewPort();
        }
    }
}
