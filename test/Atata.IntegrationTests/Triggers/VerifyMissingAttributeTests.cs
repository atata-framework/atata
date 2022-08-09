namespace Atata.IntegrationTests.Triggers;

public class VerifyMissingAttributeTests : UITestFixture
{
    [Test]
    public void Execute_WhenOnInit() =>
        Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.VerifyMissing })
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnInit_AfterGo() =>
        Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.VerifyMissing })
            .GoToWaitingOnInitPage.ClickAndGo()
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnDeInit() =>
        Go.To(new WaitingPage { NavigatingWaitKind = WaitingPage.WaitKind.VerifyMissing })
            .WaitAndGoToWaitingOnInitPage.ClickAndGo()
            .PageUrl.Should.AtOnce.EndWith(WaitingOnInitPage.Url);
}
