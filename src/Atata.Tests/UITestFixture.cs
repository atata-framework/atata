using NUnit.Framework;
using OpenQA.Selenium.Remote;

namespace Atata.Tests
{
    [TestFixture]
    public abstract class UITestFixture : UITestFixtureBase
    {
        protected virtual bool ReuseDriver => true;

        protected RemoteWebDriver PreservedDriver { get; set; }

        [SetUp]
        public void SetUp()
        {
            AtataContextBuilder contextBuilder = ConfigureBaseAtataContext();

            if (ReuseDriver && PreservedDriver != null)
                contextBuilder = contextBuilder.UseDriver(PreservedDriver);

            contextBuilder.OnDriverCreated(driver =>
            {
                if (ReuseDriver && PreservedDriver == null)
                    PreservedDriver = driver;
            });

            contextBuilder.Build();

            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
        }

        public override void TearDown()
        {
            AtataContext.Current?.CleanUp(!ReuseDriver);
        }

        [OneTimeTearDown]
        public virtual void FixtureTearDown()
        {
            CleanPreservedDriver();
        }

        private void CleanPreservedDriver()
        {
            if (PreservedDriver != null)
            {
                PreservedDriver.Dispose();
                PreservedDriver = null;
            }
        }
    }
}
