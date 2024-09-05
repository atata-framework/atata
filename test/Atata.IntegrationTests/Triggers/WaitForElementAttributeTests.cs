namespace Atata.IntegrationTests.Triggers;

public class WaitForElementAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute_WithUntilMissingOrHidden() =>
        Go.To<WaitingPage>()
            .ButtonWithMissingOrHiddenWait.Click()
            .Result.Should.AtOnce.BePresent();

    [Test]
    public void Execute_WithUntilVisibleThenHidden() =>
        Go.To<WaitingPage>()
            .ButtonWithVisibleAndHiddenWait.Click()
            .Result.Should.AtOnce.BePresent();

    [Test]
    public void Execute_WithUntilVisibleThenMissing()
    {
        WaitingPage page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            page.ButtonWithVisibleThenMissingWait.Click();

        page.Result.Should.AtOnce.BePresent();
    }

    [Test]
    public void Execute_WithUntilVisibleThenMissing_WhenNonExistent()
    {
        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(1))
            Assert.Throws<ElementNotFoundException>(
                () => page.ButtonWithVisibleThenMissingNonExistentWait.Click());
    }

    [Test]
    public void Execute_WithUntilHiddenThenVisible()
    {
        var page = Go.To<WaitingPage>();
        var control = page.ButtonWithHiddenAndVisibleWait;

        control.WaitTo().BeVisible();

        using (StopwatchAsserter.WithinSeconds(2))
            control.Click();

        page.Result.Should.AtOnce.BePresent();
    }

    [Test]
    public void Execute_WhenOnInit_AtPageObject() =>
        Go.To(new WaitingOnInitPage { OnInitWaitKind = WaitingOnInitPage.WaitKind.WaitForElementVisible })
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnInit_AtPageObject_AfterGo() =>
        Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForElementVisible })
            .GoToWaitingOnInitPage.ClickAndGo()
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnInit_AtPageObject_AfterDelayedGo() =>
        Go.To(new WaitingPage { NavigatingPageWaitKind = WaitingOnInitPage.WaitKind.WaitForElementVisible })
            .WaitAndGoToWaitingOnInitPage.ClickAndGo()
            .VerifyContentBlockIsLoaded();

    [Test]
    public void Execute_WhenOnDeInit() =>
        Go.To(new WaitingPage { NavigatingWaitKind = WaitingPage.WaitKind.WaitForElementHiddenOrMissing })
            .WaitAndGoToWaitingOnInitPage.ClickAndGo()
            .PageUrl.Should.AtOnce.EndWith(WaitingOnInitPage.Url);
}
