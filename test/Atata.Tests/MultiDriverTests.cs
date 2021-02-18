using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace Atata.Tests
{
    [TestFixture(DriverAliases.Chrome)]
    [TestFixture(HeadlessChromeAlias)]
    public class MultiDriverTests : UITestFixtureBase
    {
        private const string HeadlessChromeAlias = "chrome-headless";

        private readonly string driverAlias;

        public MultiDriverTests(string driverAlias)
        {
            this.driverAlias = driverAlias;
        }

        [SetUp]
        public void SetUp()
        {
            ConfigureBaseAtataContext()
                .UseChrome()
                    .WithAlias(HeadlessChromeAlias)
                    .WithArguments("headless")
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
            AtataContext.Current.Driver.Should().BeOfType<ChromeDriver>();
        }
    }
}
