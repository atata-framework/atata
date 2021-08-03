using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Atata.WebDriverSetup;
using NUnit.Framework;

namespace Atata.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private Process _coreRunProcess;

        [OneTimeSetUp]
        public async Task GlobalSetUpAsync()
        {
            await Task.WhenAll(
                Task.Run(SetUpDriver),
                Task.Run(SetUpTestApp));
        }

        private static void SetUpDriver() =>
            DriverSetup.AutoSetUp(BrowserNames.Chrome);

        private void SetUpTestApp()
        {
            try
            {
                PingTestApp();
            }
            catch (WebException)
            {
                RunTestApp();
            }
        }

        private static WebResponse PingTestApp() =>
            WebRequest.CreateHttp(UITestFixtureBase.BaseUrl).GetResponse();

        private void RunTestApp()
        {
            string testAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Atata.TestApp");

            _coreRunProcess = new Process
            {
                StartInfo = UITestFixtureBase.IsOSLinux
                    ? new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = "-c \"dotnet run\"",
                        WorkingDirectory = testAppPath,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                    : new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c dotnet run",
                        WorkingDirectory = testAppPath
                    }
            };

            _coreRunProcess.Start();

            Thread.Sleep(5000);

            var testAppWait = new SafeWait<SetUpFixture>(this)
            {
                Timeout = TimeSpan.FromSeconds(40),
                PollingInterval = TimeSpan.FromSeconds(1)
            };

            testAppWait.IgnoreExceptionTypes(typeof(WebException));

            testAppWait.Until(x => PingTestApp());
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            if (_coreRunProcess != null)
            {
                _coreRunProcess.Kill(true);
                _coreRunProcess.Dispose();
            }
        }
    }
}
