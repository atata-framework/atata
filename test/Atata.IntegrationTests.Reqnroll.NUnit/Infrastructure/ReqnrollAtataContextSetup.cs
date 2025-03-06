using Atata.NUnit;
using NUnit.Framework;

namespace Atata.Reqnroll.NUnit;

public static class ReqnrollAtataContextSetup
{
    public static void SetUpFeature(
        FeatureContext featureContext,
        Action<AtataContextBuilder, FeatureContext>? configure = null)
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite);

        builder.LogConsumers.RemoveAll<NUnitTestContextLogConsumer>();

        configure?.Invoke(builder, featureContext);

        AtataContext featureAtataContext = builder.Build(TestContext.CurrentContext.CancellationToken);
        featureContext.Set(featureAtataContext);
    }

    public static void TearDownFeature(FeatureContext featureContext)
    {
        if (featureContext.TryGetValue(out AtataContext featureAtataContext))
            NUnitAtataContextCompletionHandler.Complete(featureAtataContext);
    }

    public static void SetUpScenario(
        FeatureContext featureContext,
        ScenarioContext scenarioContext,
        Action<AtataContextBuilder, FeatureContext, ScenarioContext>? configure = null)
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test);

        configure?.Invoke(builder, featureContext, scenarioContext);

        AtataContext scenarioAtataContext = builder.Build(TestContext.CurrentContext.CancellationToken);
        scenarioContext.Set(scenarioAtataContext);
    }

    public static void TearDownScenario(ScenarioContext scenarioContext)
    {
        if (scenarioContext.TryGetValue(out AtataContext atataContext))
            NUnitAtataContextCompletionHandler.Complete(atataContext);
    }
}
