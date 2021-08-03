using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class AtataContextAggregateAssertTests : UITestFixture
    {
        private StubPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<StubPage>();
        }

        [Test]
        public void AtataContext_AggregateAssert_NoFailure()
        {
            AtataContext.Current.AggregateAssert(() =>
            {
                _page.IsTrue.Should.AtOnce.BeTrue();
            });
        }

        [Test]
        public void AtataContext_AggregateAssert_OneFailure()
        {
            AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
            {
                AtataContext.Current.AggregateAssert(() =>
                {
                    _page.IsTrue.Should.AtOnce.BeFalse();
                });
            });

            Assert.That(exception.Results, Has.Count.EqualTo(1));
            Assert.That(exception.Results[0].StackTrace, Does.Contain(nameof(AtataContext_AggregateAssert_OneFailure)));
            Assert.That(exception.Message, Does.StartWith("Failed with 1 assertion failure:"));
        }

        [Test]
        public void AtataContext_AggregateAssert_TwoFailures()
        {
            AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
            {
                AtataContext.Current.AggregateAssert(() =>
                {
                    _page.IsTrue.Should.AtOnce.BeFalse();
                    _page.IsTrue.Should.AtOnce.BeTrue();
                    _page.IsTrue.Should.AtOnce.BeFalse();
                });
            });

            Assert.That(exception.Results, Has.Count.EqualTo(2));
            Assert.That(exception.Results.Select(x => x.StackTrace), Has.All.Contain(nameof(AtataContext_AggregateAssert_TwoFailures)));
            Assert.That(exception.Message, Does.StartWith("Failed with 2 assertion failures:"));
        }
    }
}
