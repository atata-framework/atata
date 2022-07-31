using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Atata.IntegrationTests
{
    public class WaitToTests : UITestFixture
    {
        private StubPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<StubPage>();
        }

        [Test]
        public void WaitTo_NoFailure()
        {
            var waitTo = _page.IsTrue.WaitTo;

            waitTo.BeTrue();
        }

        [Test]
        public void WaitTo_NoFailure_WithRetry()
        {
            var waitTo = _page.IsTrueInASecond.WaitTo;

            waitTo.BeTrue();
        }

        [Test]
        public void WaitTo_Positive_Failure()
        {
            var waitTo = _page.IsTrue.WaitTo.AtOnce;

            TimeoutException exception = Assert.Throws<TimeoutException>(() =>
                waitTo.BeFalse());

            Assert.That(exception.Message, Does.StartWith("Timed out waiting for "));
            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public void WaitTo_Negative_Failure()
        {
            var waitTo = _page.IsTrue.WaitTo.Not.AtOnce;

            TimeoutException exception = Assert.Throws<TimeoutException>(() =>
                waitTo.BeTrue());

            Assert.That(exception.Message, Does.StartWith("Timed out waiting for "));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
