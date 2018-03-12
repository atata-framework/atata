using System;
using NUnit.Framework;

namespace Atata.Tests
{
    public class DefaultTimeoutTests : UITestFixtureBase
    {
        [Test]
        public void DefaultTimeout_Waiting()
        {
            ConfigureBaseAtataContext().
                UseWaitingTimeout(TimeSpan.FromSeconds(3)).
                Build();

            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.Within(3))
                page.MissingControl.Wait(Until.Visible, new WaitOptions { ThrowOnPresenceFailure = false });
        }
    }
}
