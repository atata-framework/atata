using NUnit.Framework;

namespace Atata.Tests
{
    public class WaitingOnInitTests : UITestFixture
    {
        [Test]
        public void Trigger_VerifyExists()
        {
            var page = Go.To(new WaitingOnInitPage(WaitingOnInitPage.WaitKind.VerifyExists));

            Assert.That(page.ContentBlock.GetScope(SearchOptions.UnsafelyAtOnce()).Text, Is.EqualTo("Loaded"));
        }

        [Test]
        public void Trigger_VerifyMissing()
        {
            var page = Go.To(new WaitingOnInitPage(WaitingOnInitPage.WaitKind.VerifyMissing));

            Assert.That(page.ContentBlock.GetScope(SearchOptions.UnsafelyAtOnce()).Text, Is.EqualTo("Loaded"));
        }

        [Test]
        public void WaitForElement_OnInit_AtPageObject()
        {
            var page = Go.To(new WaitingOnInitPage(WaitingOnInitPage.WaitKind.WaitForElement));

            Assert.That(page.ContentBlock.GetScope(SearchOptions.UnsafelyAtOnce()).Text, Is.EqualTo("Loaded"));
        }

        [Test]
        public void WaitForElement_OnInit_AtPageObject_AfterGo()
        {
            var page = Go.To<WaitingPage>().
                GoToWaitingOnInitPage.ClickAndGo();

            Assert.That(page.ContentBlock.GetScope(SearchOptions.UnsafelyAtOnce()).Text, Is.EqualTo("Loaded"));
        }

        [Test]
        public void WaitForElement_OnInit_AtPageObject_AfterWaitAndGo()
        {
            var page = Go.To<WaitingPage>().
                WaitAndGoToWaitingOnInitPage.ClickAndGo();

            Assert.That(page.ContentBlock.GetScope(SearchOptions.UnsafelyAtOnce()).Text, Is.EqualTo("Loaded"));
        }
    }
}
