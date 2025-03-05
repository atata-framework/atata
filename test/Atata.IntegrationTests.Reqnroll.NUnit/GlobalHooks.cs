using Atata;
using Atata.Reqnroll.NUnit;

namespace Atata_IntegrationTests.Reqnroll_NUnit;

[Binding]
public sealed class GlobalHooks
{
    [BeforeFeature]
    public static void SetUpFeature(FeatureContext featureContext) =>
        ReqnrollAtataContextSetup.SetUpFeature(featureContext, ConfigureFeatureAtataContext);

    [AfterFeature]
    public static void TearDownFeature(FeatureContext featureContext) =>
        ReqnrollAtataContextSetup.TearDownFeature(featureContext);

    [BeforeScenario]
    public void SetUpScenario(FeatureContext featureContext, ScenarioContext scenarioContext) =>
        ReqnrollAtataContextSetup.SetUpScenario(featureContext, scenarioContext, ConfigureScenarioAtataContext);

    [AfterScenario]
    public void TearDownScenario(ScenarioContext scenarioContext) =>
        ReqnrollAtataContextSetup.TearDownScenario(scenarioContext);

    private static void ConfigureFeatureAtataContext(
        AtataContextBuilder builder,
        FeatureContext featureContext)
    {
        // TODO: Add extra configuration for feature AtataContext.
    }

    private static void ConfigureScenarioAtataContext(
        AtataContextBuilder builder,
        FeatureContext featureContext,
        ScenarioContext scenarioContext)
    {
        // TODO: Add extra configuration for scenario AtataContext.
    }
}
