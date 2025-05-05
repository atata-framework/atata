namespace Atata.Reqnroll.NUnit;

public static class ReqnrollAtataContextSetup
{
    public static void SetUpFeature(
        FeatureContext featureContext,
        Action<AtataContextBuilder, FeatureContext>? configure = null)
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite)
            .UseDefaultCancellationToken(TestContext.CurrentContext.CancellationToken);

        builder.LogConsumers.RemoveAll<NUnitTestContextLogConsumer>();

        configure?.Invoke(builder, featureContext);

        AtataContext featureAtataContext = builder.Build();
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
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseDefaultCancellationToken(TestContext.CurrentContext.CancellationToken);

        configure?.Invoke(builder, featureContext, scenarioContext);

        AtataContext scenarioAtataContext = builder.Build();
        scenarioContext.Set(scenarioAtataContext);
    }

    public static void TearDownScenario(ScenarioContext scenarioContext)
    {
        if (scenarioContext.TryGetValue(out AtataContext atataContext))
            NUnitAtataContextCompletionHandler.Complete(atataContext);
    }
}
