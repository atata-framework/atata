using NUnit.Framework;

namespace Atata.Tests
{
    public class VerifyMissingTests : UITestFixture
    {
        [Test]
        public void Trigger_VerifyMissing_OnInit()
        {
            Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.VerifyMissing }).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_VerifyMissing_OnInit_AfterGo()
        {
            Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.VerifyMissing }).
                GoToWaitingOnInitPage.ClickAndGo().
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_VerifyMissing_OnDeInit()
        {
            Go.To(new WaitingPage { NavigatingWaitKind = WaitingPage.WaitKind.VerifyMissing }).
                WaitAndGoToWaitingOnInitPage.ClickAndGo().
                PageUrl.Should.AtOnce.EndWith(WaitingOnInitPage.Url);
        }
    }
}
