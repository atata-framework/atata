using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using NUnit.Framework;
using Retry;

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
            coreRunProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c dotnet run",
                    WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\Atata.TestApp")
                }
            };

            coreRunProcess.Start();

            Thread.Sleep(5000);

            RetryHelper.Instance.Try(() => PingTestApp()).
                WithTryInterval(1000).
                WithTimeLimit(40000).
                UntilNoException();
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            coreRunProcess?.CloseMainWindow();
        }
    }
}
