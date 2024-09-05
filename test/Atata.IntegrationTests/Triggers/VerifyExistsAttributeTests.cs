namespace Atata.IntegrationTests.Triggers;

public class VerifyExistsAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute_WhenOnInit() =>
        Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.VerifyExists })
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnInit_AfterGo() =>
        Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.VerifyExists })
            .GoToWaitingOnInitPage.ClickAndGo()
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnInit_AfterDelayedGo() =>
        Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.VerifyExists })
            .WaitAndGoToWaitingOnInitPage.ClickAndGo()
            .VerifyContentBlockIsLoaded();
}
