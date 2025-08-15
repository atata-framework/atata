namespace Atata.IntegrationTests.WebDriverSetup;

public sealed class SetUpWebDriversForUseEventHandlerTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public async Task WhenOneDriverIsConfigured()
    {
        await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.AddWebDriver(x => x
                .UseChrome(x => x
                    .WithArguments(ChromeArguments)))
            .EventSubscriptions.Add(SetUpWebDriversForUseEventHandler.Instance)
            .BuildAsync();
    }

    [Test]
    public async Task WhenTwoDriversAreConfigured()
    {
        await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.AddWebDriver(x => x
                .UseEdge()
                .UseChrome(x => x
                    .WithArguments(ChromeArguments)))
            .EventSubscriptions.Add(SetUpWebDriversForUseEventHandler.Instance)
            .BuildAsync();
    }
}
