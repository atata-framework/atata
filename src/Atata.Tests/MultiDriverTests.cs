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
            ConfigureBaseAtataContext()
                .UseInternetExplorer()
                .UseDriver(driverAlias)
                .UseTestName(() => $"[{driverAlias}]{TestContext.CurrentContext.Test.Name}")
                .Build();
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
            return driverAlias switch
            {
                DriverAliases.Chrome => typeof(ChromeDriver),
                DriverAliases.InternetExplorer => typeof(InternetExplorerDriver),
                _ => throw new ArgumentException($"Unexpected \"{driverAlias}\" value.", nameof(driverAlias)),
            };
        }
    }
}
