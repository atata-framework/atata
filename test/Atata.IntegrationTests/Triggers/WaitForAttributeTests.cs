namespace Atata.IntegrationTests.Triggers;

public class WaitForAttributeTests : UITestFixture
{
    [Test]
    public void Execute_WhenOnInit_AtPageObject() =>
        Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.WaitForVisible })
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnInit_AtPageObject_AfterGo() =>
        Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForVisible })
            .GoToWaitingOnInitPage.ClickAndGo()
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnInit_AtPageObject_AfterDelayedGo() =>
        Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForVisible })
            .WaitAndGoToWaitingOnInitPage.ClickAndGo()
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnDeInit() =>
        Go.To(new WaitingPage { NavigatingWaitKind = WaitingPage.WaitKind.WaitForMissing })
            .WaitAndGoToWaitingOnInitPage.ClickAndGo()
            .PageUrl.Should.AtOnce.EndWith(WaitingOnInitPage.Url);
}
