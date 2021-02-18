using NUnit.Framework;

namespace Atata.Tests
{
    public class AtataContextTests : UITestFixture
    {
        protected override bool ReuseDriver => false;

        [Test]
        public void AtataContext_RestartDriver()
        {
            AtataContext.Current.RestartDriver();

            Go.To<BasicControlsPage>();
            Assert.That(AtataContext.Current.Driver.Title, Is.Not.Null.Or.Empty);

            AtataContext.Current.RestartDriver();

            Assert.That(AtataContext.Current.Driver.Title, Is.Null.Or.Empty);
            Go.To<BasicControlsPage>();
        }
    }
}
