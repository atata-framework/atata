using System;
using NUnit.Framework;

namespace Atata.Tests
{
    [Parallelizable(ParallelScope.None)]
    public class DefaultTimeoutTests : UITestFixtureBase
    {
        [Test]
        public void DefaultTimeout_BaseRetryTimeout_ElementFind()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(2)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(2))
                page.MissingControl.GetScope();
        }

        [Test]
        public void DefaultTimeout_BaseRetryTimeout_Waiting()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(2)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(2))
                page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
        }

        [Test]
        public void DefaultTimeout_BaseRetryTimeout_Verification()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(2)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(2))
                Assert.Throws<AssertionException>(() =>
                    page.MissingControl.Should.BeEnabled());
        }

        [Test]
        public void DefaultTimeout_ElementFindTimeout()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(1)).
                UseElementFindTimeout(TimeSpan.FromSeconds(3)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(3))
                page.MissingControl.GetScope();
        }

        [Test]
        public void DefaultTimeout_WaitingTimeout()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(1)).
                UseWaitingTimeout(TimeSpan.FromSeconds(3)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(3))
                page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
        }

        [Test]
        public void DefaultTimeout_VerificationTimeout_DoesExist()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(1)).
                UseVerificationTimeout(TimeSpan.FromSeconds(3)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(3))
                AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
                    page.MissingControl.Should.Exist());
        }

        [Test]
        public void DefaultTimeout_VerificationTimeout_CheckDataProperty()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(1)).
                UseVerificationTimeout(TimeSpan.FromSeconds(3)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(3))
                Assert.Throws<AssertionException>(() =>
                    page.MissingControl.IsEnabled.Should.BeTrue());
        }
    }
}
