using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class DefaultTimeoutTests : UITestFixtureBase
    {
        [Test]
        public void DefaultTimeout_BaseRetryTimeout_ElementFind()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(2)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.Within(2))
                page.MissingControl.GetScope();
        }

        [Test]
        public void DefaultTimeout_BaseRetryTimeout_Waiting()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(2)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.Within(2))
                page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
        }

        [Test]
        public void DefaultTimeout_BaseRetryTimeout_Verification()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(2)).
                Build();

            var page = Go.To<WaitingPage>();

            Assert.Throws<NoSuchElementException>(() =>
            {
                using (StopwatchAsserter.Within(2))
                    page.MissingControl.Should.BeEnabled();
            });
        }

        [Test]
        public void DefaultTimeout_WaitingTimeout()
        {
            ConfigureBaseAtataContext().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(1)).
                UseWaitingTimeout(TimeSpan.FromSeconds(3)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.Within(3))
                page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
        }
    }
}
