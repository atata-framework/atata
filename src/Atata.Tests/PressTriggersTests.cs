using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class PressTriggersTests : UITestFixture
    {
        [Test]
        public void PressEnd_BeforeClick()
        {
            var page = Go.To<PressTriggersPage>();

            Assert.That(IsVisibleInViewport(page.BottomText), Is.False);

            page.BottomText.Click();

            Assert.That(IsVisibleInViewport(page.BottomText), Is.True);
        }

        [Test]
        public void PressHome_BeforeClick()
        {
            var page = Go.To<PressTriggersPage>();

            page.Press(Keys.End);

            Assert.That(IsVisibleInViewport(page.TopText), Is.False);

            page.TopText.Click();

            Assert.That(IsVisibleInViewport(page.TopText), Is.True);
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
