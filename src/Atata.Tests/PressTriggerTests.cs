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
            IsVisibleInViewport(page.BottomText).Should().BeFalse();

            page.BottomText.Click();

            IsVisibleInViewport(page.BottomText).Should().BeTrue();
        }

        [Test]
        public void PressHome_BeforeClick()
        {
            page.Press(Keys.End);

            IsVisibleInViewport(page.TopText).Should().BeFalse();

            page.TopText.Click();

            IsVisibleInViewport(page.TopText).Should().BeTrue();
        }

        private static bool IsVisibleInViewport<T>(Control<T> element)
            where T : PageObject<T>
        {
            return (bool)((IJavaScriptExecutor)AtataContext.Current.Driver).ExecuteScript(
                "var elem = arguments[0],                 " +
                "  box = elem.getBoundingClientRect(),    " +
                "  cx = box.left + box.width / 2,         " +
                "  cy = box.top + box.height / 2,         " +
                "  e = document.elementFromPoint(cx, cy); " +
                "for (; e; e = e.parentElement) {         " +
                "  if (e === elem)                        " +
                "    return true;                         " +
                "}                                        " +
                "return false;                            ",
                element.Scope);
        }
    }
}
