using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class WaitForElementTests : UITestFixture
    {
        [Test]
        public void Trigger_WaitForElement_MissingOrHidden()
        {
            Go.To<WaitingPage>().
                ButtonWithMissingOrHiddenWait.Click().
                Result.Should.AtOnce.Exist();
        }

        [Test]
        public void Trigger_WaitForElement_VisibleThenHidden()
        {
            Go.To<WaitingPage>().
                ButtonWithVisibleAndHiddenWait.Click().
                Result.Should.AtOnce.Exist();
        }

        [Test]
        public void Trigger_WaitForElement_VisibleThenMissing()
        {
            WaitingPage page = Go.To<WaitingPage>();

            using (StopwatchAsserter.Within(2))
                page.ButtonWithVisibleAndMissingWait.Click();

            page.Result.Should.AtOnce.Exist();
        }

        [Test]
        public void Trigger_WaitForElement_VisibleAndMissing_NonExistent()
        {
            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.Within(1))
                Assert.Throws<NoSuchElementException>(
                    () => page.ButtonWithVisibleAndMissingNonExistentWait.Click());
        }

        [Test]
        public void Trigger_WaitForElement_HiddenThenVisible()
        {
            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.Within(2))
                page.ButtonWithHiddenAndVisibleWait.Click();

            page.Result.Should.AtOnce.Exist();
        }

        [Test]
        public void Trigger_WaitForElement_OnInit_AtPageObject()
        {
            Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.WaitForElementVisible }).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_WaitForElement_OnInit_AtPageObject_AfterGo()
        {
            Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForElementVisible }).
                GoToWaitingOnInitPage.ClickAndGo().
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_WaitForElement_OnInit_AtPageObject_AfterDelayedGo()
        {
            Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForElementVisible }).
                WaitAndGoToWaitingOnInitPage.ClickAndGo().
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_WaitForElement_OnDeInit()
        {
            Go.To(new WaitingPage { NavigatingWaitKind = WaitingPage.WaitKind.WaitForElementHiddenOrMissing }).
                WaitAndGoToWaitingOnInitPage.ClickAndGo().
                PageUrl.Should.AtOnce.EndWith(WaitingOnInitPage.Url);
        }
    }
}
