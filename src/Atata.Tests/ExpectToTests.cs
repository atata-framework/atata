using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Atata.Tests
{
    public class ExpectToTests : UITestFixture
    {
        private StubPage page;

        protected override void OnSetUp()
        {
            page = Go.To<StubPage>();
        }

        [Test]
        public void ExpectTo_NoFailure()
        {
            var expectTo = page.IsTrue.ExpectTo.AtOnce;

            expectTo.BeTrue();

            AtataContext.Current.CleanUp(false);
        }

        [Test]
        public void ExpectTo_OneFailure()
        {
            var expectTo = page.IsTrue.ExpectTo.AtOnce;

            expectTo.BeFalse();

            AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
                AtataContext.Current.CleanUp(false));

            Assert.That(exception.Results, Has.Count.EqualTo(1));
            Assert.That(exception.Results[0].StackTrace, Does.Contain(nameof(ExpectTo_OneFailure)));
            Assert.That(exception.Message, Does.StartWith("Failed with 1 assertion failure:"));
        }

        [Test]
        public void ExpectTo_TwoFailures()
        {
            var expectTo = page.IsTrue.ExpectTo.AtOnce;

            expectTo.BeFalse();
            expectTo.BeFalse();

            AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
                AtataContext.Current.CleanUp(false));

            Assert.That(exception.Results, Has.Count.EqualTo(2));
            Assert.That(exception.Results.Select(x => x.StackTrace), Has.All.Contain(nameof(ExpectTo_TwoFailures)));
            Assert.That(exception.Message, Does.StartWith("Failed with 2 assertion failures:"));
        }
    }
}
