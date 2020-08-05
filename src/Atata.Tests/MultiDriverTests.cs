using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace Atata.Tests
{
    [TestFixture(DriverAliases.Chrome)]
    [TestFixture(DriverAliases.InternetExplorer)]
    public class MultiDriverTests : UITestFixtureBase
    {
        private readonly string driverAlias;

        public MultiDriverTests(string driverAlias)
        {
            this.driverAlias = driverAlias;
        }

        [SetUp]
        public void SetUp()
        {
            ConfigureBaseAtataContext().
                UseInternetExplorer().
#if NETCOREAPP2_1
                    WithLocalDriverPath().
#endif
                UseDriver(driverAlias).
                UseTestName(() => $"[{driverAlias}]{TestContext.CurrentContext.Test.Name}").
                Build();
        }

        [TestCase(4)]
        [TestCase(8)]
        public void MultiDriver_WithParameter(int parameter)
        {
            AtataContext.Current.Log.Info($"Driver alias: {driverAlias}");
            AtataContext.Current.Log.Info($"Parameter value: {parameter}");

            AtataContext.Current.DriverAlias.Should().Be(driverAlias);
            AtataContext.Current.Driver.Should().BeOfType(GetDriverTypeByAlias(driverAlias));
        }

        private static Type GetDriverTypeByAlias(string driverAlias)
        {
            switch (driverAlias)
            {
                case DriverAliases.Chrome:
                    return typeof(ChromeDriver);
                case DriverAliases.InternetExplorer:
                    return typeof(InternetExplorerDriver);
                default:
                    throw new ArgumentException($"Unexpected \"{driverAlias}\" value.", nameof(driverAlias));
            }
        }
    }
}
