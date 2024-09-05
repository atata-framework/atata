namespace Atata.IntegrationTests.Context;

[Parallelizable(ParallelScope.None)]
public class AtataContextTimeoutTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void BaseRetryTimeout_ElementFind()
    {
        ConfigureAtataContextWithWebDriverSession()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(2))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(2))
            page.MissingControl.GetScope();
    }

    [Test]
    public void BaseRetryTimeout_Waiting()
    {
        ConfigureAtataContextWithWebDriverSession()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(2))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(2))
            page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
    }

    [Test]
    public void BaseRetryTimeout_Verification()
    {
        ConfigureAtataContextWithWebDriverSession()
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
        ConfigureAtataContextWithWebDriverSession(
            x => x.UseElementFindTimeout(TimeSpan.FromSeconds(3)))
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(1))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            page.MissingControl.GetScope();
    }

    [Test]
    public void WaitingTimeout()
    {
        ConfigureAtataContextWithWebDriverSession()
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
        ConfigureAtataContextWithWebDriverSession()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(1))
            .UseVerificationTimeout(TimeSpan.FromSeconds(3))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
                page.MissingControl.Should.BePresent());
    }

    [Test]
    public void VerificationTimeout_ForValueProvider()
    {
        ConfigureAtataContextWithWebDriverSession()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(1))
            .UseVerificationTimeout(TimeSpan.FromSeconds(3))
            .Build();

        var page = Go.To<WaitingPage>();

        using (StopwatchAsserter.WithinSeconds(3))
            Assert.Throws<AssertionException>(() =>
                page.MissingControl.IsEnabled.Should.BeTrue());
    }
}
