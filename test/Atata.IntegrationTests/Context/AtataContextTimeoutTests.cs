namespace Atata.IntegrationTests.Context;

[Parallelizable(ParallelScope.None)]
public class AtataContextTimeoutTests : UITestFixtureBase
{
    [Test]
    public void BaseRetryTimeout_ElementFind()
    {
        ConfigureBaseAtataContext()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(2))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(2))
            page.MissingControl.GetScope();
    }

    [Test]
    public void BaseRetryTimeout_Waiting()
    {
        ConfigureBaseAtataContext()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(2))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(2))
            page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
    }

    [Test]
    public void BaseRetryTimeout_Verification()
    {
        ConfigureBaseAtataContext()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(2))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(2))
            Assert.Throws<AssertionException>(() =>
                page.MissingControl.Should.BeEnabled());
    }

    [Test]
    public void ElementFindTimeout()
    {
        ConfigureBaseAtataContext()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(1))
            .UseElementFindTimeout(TimeSpan.FromSeconds(3))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            page.MissingControl.GetScope();
    }

    [Test]
    public void WaitingTimeout()
    {
        ConfigureBaseAtataContext()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(1))
            .UseWaitingTimeout(TimeSpan.FromSeconds(3))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
    }

    [Test]
    public void VerificationTimeout_ForControl()
    {
        ConfigureBaseAtataContext()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(1))
            .UseVerificationTimeout(TimeSpan.FromSeconds(3))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
                page.MissingControl.Should.Exist());
    }

    [Test]
    public void VerificationTimeout_ForValueProvider()
    {
        ConfigureBaseAtataContext()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(1))
            .UseVerificationTimeout(TimeSpan.FromSeconds(3))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            Assert.Throws<AssertionException>(() =>
                page.MissingControl.IsEnabled.Should.BeTrue());
    }
}
