using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Atata.TestApp;
using Atata.WebDriverSetup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using NUnit.Framework;

namespace Atata.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private IWebHost _testAppWebHost;

        [OneTimeSetUp]
        public async Task GlobalSetUpAsync()
        {
            await Task.WhenAll(
                DriverSetup.AutoSetUpAsync(BrowserNames.Chrome),
                SetUpTestAppAsync());
        }

        private static bool IsTestAppRunningOnDefaultPort()
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            return ipEndPoints.Any(x => x.Port == UITestFixtureBase.DefaultTestAppPort);
        }

        private async Task SetUpTestAppAsync()
        {
            if (!IsTestAppRunningOnDefaultPort())
                await StartTestAppAsync();
        }

        private async Task StartTestAppAsync()
        {
            _testAppWebHost = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();

            await _testAppWebHost.StartAsync();

            var webHostUrls = _testAppWebHost.ServerFeatures
                .Get<IServerAddressesFeature>()
                .Addresses;

            UITestFixtureBase.BaseUrl = webHostUrls.First();
        }

        [OneTimeTearDown]
        public async Task GlobalTearDownAsync()
        {
            if (_testAppWebHost != null)
                await _testAppWebHost.StopAsync();
        }
    }
}
