using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using NUnit.Framework;

namespace Atata.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private Process coreRunProcess;

        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            try
            {
                PingTestApp();
            }
            catch
            {
                RunTestApp();
            }
        }

        private static WebResponse PingTestApp() =>
            WebRequest.CreateHttp(UITestFixtureBase.BaseUrl).GetResponse();

        private void RunTestApp()
        {
            bool isLinux = false;

#if NETCOREAPP2_0
            isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
#endif

            string testAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Atata.TestApp");

            coreRunProcess = new Process
            {
                StartInfo = isLinux
                    ? new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = "-c dotnet run",
                        WorkingDirectory = testAppPath
                    }
                    : new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c dotnet run",
                        WorkingDirectory = testAppPath
                    }
            };

            coreRunProcess.Start();

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
            coreRunProcess?.CloseMainWindow();
            coreRunProcess?.Dispose();
        }
    }
}
