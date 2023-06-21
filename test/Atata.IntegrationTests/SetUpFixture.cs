using System.Net.NetworkInformation;
using Atata.Cli;
using Atata.WebDriverSetup;

namespace Atata.IntegrationTests;

[SetUpFixture]
public class SetUpFixture
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
            x => x.Port == UITestFixtureBase.TestAppPort);

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

        var testAppWait = new SafeWait<SetUpFixture>(this)
        {
            Timeout = TimeSpan.FromSeconds(40),
            PollingInterval = TimeSpan.FromSeconds(0.2)
        };

        testAppWait.Until(x => IsTestAppRunning());
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
