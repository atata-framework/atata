using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Atata.Tests
{
    public class WaitToTests : UITestFixture
    {
        private StubPage page;

        protected override void OnSetUp()
        {
            page = Go.To<StubPage>();
        }

        [Test]
        public void WaitTo_NoFailure()
        {
            var waitTo = page.IsTrue.WaitTo;

            waitTo.BeTrue();
        }

        [Test]
        public void WaitTo_NoFailure_WithRetry()
        {
            var waitTo = page.IsTrueInASecond.WaitTo;

            waitTo.BeTrue();
        }

        [Test]
        public void WaitTo_OneFailure()
        {
            var waitTo = page.IsTrue.WaitTo.AtOnce;

            TimeoutException exception = Assert.Throws<TimeoutException>(() =>
                waitTo.BeFalse());

            Assert.That(exception.Message, Does.StartWith("Timed out waiting for "));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
