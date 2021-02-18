using NUnit.Framework;

namespace Atata.Tests
{
    public class VerifyExistsTests : UITestFixture
    {
        [Test]
        public void Trigger_VerifyExists_OnInit()
        {
            Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.VerifyExists }).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_VerifyExists_OnInit_AfterGo()
        {
            Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.VerifyExists }).
                GoToWaitingOnInitPage.ClickAndGo().
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void Trigger_VerifyExists_OnInit_AfterDelayedGo()
        {
            Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.VerifyExists }).
                WaitAndGoToWaitingOnInitPage.ClickAndGo().
                VerifyContentBlockIsLoaded();
        }
    }
}
