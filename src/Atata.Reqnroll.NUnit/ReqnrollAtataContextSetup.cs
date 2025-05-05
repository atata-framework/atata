namespace Atata.Reqnroll.NUnit;

/// <summary>
/// Provides setup and tear-down methods for <see cref="AtataContext"/> in the context of Reqnroll.NUnit features and scenarios.
/// </summary>
public static class ReqnrollAtataContextSetup
{
    /// <summary>
    /// Sets up the <see cref="AtataContext"/> for the feature.
    /// </summary>
    /// <param name="featureContext">The context of the feature.</param>
    /// <param name="configure">An action delegate to configure the feature <see cref="AtataContextBuilder"/>.</param>
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

    /// <summary>
    /// Tears down the <see cref="AtataContext"/> for the feature.
    /// </summary>
    /// <param name="featureContext">The context of the feature.</param>
    public static void TearDownFeature(FeatureContext featureContext)
    {
        if (featureContext.TryGetValue(out AtataContext featureAtataContext))
            NUnitAtataContextCompletionHandler.Complete(featureAtataContext);
    }

    /// <summary>
    /// Sets up the <see cref="AtataContext"/> for the scenario.
    /// </summary>
    /// <param name="featureContext">The context of the feature.</param>
    /// <param name="scenarioContext">The context of the scenario.</param>
    /// <param name="configure">An action delegate to configure the scenario <see cref="AtataContextBuilder"/>.</param>
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

    /// <summary>
    /// Tears down the <see cref="AtataContext"/> for the scenario.
    /// </summary>
    /// <param name="scenarioContext">The context of the scenario.</param>
    public static void TearDownScenario(ScenarioContext scenarioContext)
    {
        if (scenarioContext.TryGetValue(out AtataContext atataContext))
            NUnitAtataContextCompletionHandler.Complete(atataContext);
    }
}
