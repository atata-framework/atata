using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture(DriverAliases.Chrome)]
    [TestFixture(DriverAliases.InternetExplorer)]
    public class MultiDriverTests
    {
        private readonly string driverAlias;

        public MultiDriverTests(string driverAlias)
        {
            this.driverAlias = driverAlias;
        }

        [SetUp]
        public void SetUp()
        {
            AtataContext.Configure().
#if NETCOREAPP2_0
                UseChrome().
                    WithFixOfCommandExecutionDelay().
                    WithDriverPath(System.AppDomain.CurrentDomain.BaseDirectory).
                UseInternetExplorer().
                    WithFixOfCommandExecutionDelay().
                    WithDriverPath(System.AppDomain.CurrentDomain.BaseDirectory).
#endif
                UseDriver(driverAlias).
                UseTestName(() => $"[{driverAlias}]{TestContext.CurrentContext.Test.Name}").
                AddNUnitTestContextLogging().
                LogNUnitError().
                Build();
        }

        [TearDown]
        public void TearDown()
        {
            AtataContext.Current.CleanUp();
        }

        [TestCase(4)]
        [TestCase(8)]
        public void MultiDriver_WithParameter(int parameter)
        {
            AtataContext.Current.Log.Info($"Driver alias: {driverAlias}");
            AtataContext.Current.Log.Info($"Parameter value: {parameter}");
        }
    }
}
