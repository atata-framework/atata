using NUnit.Framework;

namespace Atata.Tests
{
    public class WaitForTests : UITestFixture
    {
        [Test]
        public void Trigger_WaitFor_OnInit_AtPageObject()
        {
            Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.WaitForVisible }).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_WaitFor_OnInit_AtPageObject_AfterGo()
        {
            Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForVisible }).
                GoToWaitingOnInitPage.ClickAndGo().
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_WaitFor_OnInit_AtPageObject_AfterDelayedGo()
        {
            Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForVisible }).
                WaitAndGoToWaitingOnInitPage.ClickAndGo().
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_WaitFor_OnDeInit()
        {
            Go.To(new WaitingPage { NavigatingWaitKind = WaitingPage.WaitKind.WaitForMissing }).
                WaitAndGoToWaitingOnInitPage.ClickAndGo().
                PageUrl.Should.AtOnce.EndWith(WaitingOnInitPage.Url);
        }
    }
}
