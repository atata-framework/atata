using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class WaitingTests : UITestFixture
    {
        private WaitingPage page;

        protected override void OnSetUp()
        {
            page = Go.To<WaitingPage>();
        }

        [Test]
        public void Waiting_BuiltIn_Disabled()
        {
            page.
                ButtonWithoutWait.Click();

            Assert.Throws<NoSuchElementException>(
                () => page.Result.Should.AtOnce.Exist());
        }

        [Test]
        public void Waiting_BuiltIn()
        {
            page.
                ButtonWithoutWait.Click().
                Result.Should.Exist();
        }

        [Test]
        public void WaitForElement_MissingOrHidden()
        {
            page.
                ButtonWithMissingOrHiddenWait.Click().
                Result.Should.AtOnce.Exist();
        }

        [Test]
        public void WaitForElement_VisibleAndHidden()
        {
            page.
                ButtonWithVisibleAndHiddenWait.Click().
                Result.Should.AtOnce.Exist();
        }

        [Test]
        public void WaitForElement_VisibleAndMissing()
        {
            using (StopwatchAsserter.Within(2))
                page.ButtonWithVisibleAndMissingWait.Click();

            page.Result.Should.AtOnce.Exist();
        }

        [Test]
        public void WaitForElement_VisibleAndMissing_NonExistent()
        {
            using (StopwatchAsserter.Within(1))
                Assert.Throws<NoSuchElementException>(
                    () => page.ButtonWithVisibleAndMissingNonExistentWait.Click());
        }

        [Test]
        public void WaitForElement_HiddenAndVisible()
        {
            using (StopwatchAsserter.Within(2))
                page.ButtonWithHiddenAndVisibleWait.Click();

            page.Result.Should.AtOnce.Exist();
        }
    }
}
