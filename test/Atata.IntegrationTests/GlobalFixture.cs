using System.Net.NetworkInformation;
using Atata.Cli;
using Atata.WebDriverSetup;

namespace Atata.IntegrationTests;

[SetUpFixture]
public class GlobalFixture
{
    private CliCommand _dotnetRunCommand;

    [OneTimeSetUp]
    public async Task GlobalSetUpAsync() =>
        await Task.WhenAll(
            DriverSetup.AutoSetUpAsync(BrowserNames.Chrome),
            Task.Run(SetUpTestApp));

    private static bool IsTestAppRunning() =>
        Array.Exists(
            IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners(),
            x => x.Port == WebDriverSessionTestSuiteBase.TestAppPort);

    private void SetUpTestApp()
    {
        if (!IsTestAppRunning())
            StartTestApp();
    }

    private void StartTestApp()
    {
        string testAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Atata.TestApp");

        ProgramCli dotnetCli = new ProgramCli("dotnet", useCommandShell: true)
            .WithWorkingDirectory(testAppPath);

        _dotnetRunCommand = dotnetCli.Start("run");

        RetryWait testAppWait = new(TimeSpan.FromSeconds(40), TimeSpan.FromSeconds(0.2));
        testAppWait.Until(IsTestAppRunning);
    }

    [OneTimeTearDown]
    public void GlobalTearDown()
    {
        if (_dotnetRunCommand != null)
        {
            _dotnetRunCommand.Kill(true);
            _dotnetRunCommand.Dispose();
        }
    }
}
