using System;
using NUnit.Framework;

namespace Atata.Tests
{
    public class WaitForScriptTests : UITestFixture
    {
        [Test]
        public void Trigger_WaitForScript()
        {
            Go.To<WaitingPage>().
                ButtonWithSuccessfulScriptWait().
                ValueBlock.Should.AtOnce.Exist();
        }

        [Test]
        public void Trigger_WaitForScript_Timeout()
        {
            var page = Go.To<WaitingPage>();

            using (StopwatchAsserter.WithinSeconds(1))
                Assert.Throws<TimeoutException>(
                    () => page.ButtonWithTimeoutScriptWait());
        }
    }
}
