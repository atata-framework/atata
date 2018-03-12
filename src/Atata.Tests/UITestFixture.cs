using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public abstract class UITestFixture : UITestFixtureBase
    {
        [SetUp]
        public void SetUp()
        {
            ConfigureBaseAtataContext().
                Build();

            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
        }
    }
}
