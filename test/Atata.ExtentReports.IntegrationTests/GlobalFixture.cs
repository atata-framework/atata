namespace Atata.ExtentReports.IntegrationTests;

[SetUpFixture]
public class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties) =>
        globalProperties.UseRootNamespaceOf<GlobalFixture>();

    protected override void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder) =>
        builder
            .UseCulture("en-US")
            .Sessions.AddWebDriver(x => x
                .UseStartScopes(AtataContextScopes.Test)
                .UseBaseUrl("https://demo.atata.io/")
                .UseChrome(x => x
                    .WithArguments("headless=new", "window-size=1024,768", "disable-search-engine-choice-screen")))
            .LogConsumers.AddNLogFile()
            .UseExtentReports();

    protected override void ConfigureAtataContext(AtataContextBuilder builder) =>
        builder.EventSubscriptions.Add(SetUpWebDriversForUseEventHandler.Instance);
}
