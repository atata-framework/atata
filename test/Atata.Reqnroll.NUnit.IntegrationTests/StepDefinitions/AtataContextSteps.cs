using Atata_Reqnroll_NUnit_IntegrationTests.Features;

namespace Atata_Reqnroll_NUnit_IntegrationTests;

[Binding]
public sealed class AtataContextSteps
{
    private readonly ScenarioContext _scenarioContext;

    public AtataContextSteps(ScenarioContext scenarioContext) =>
        _scenarioContext = scenarioContext;

    [Then("Scenario AtataContext is AtataContext.Current")]
    public void ThenScenarioAtataContextIsAtataContextCurrent() =>
        _scenarioContext.Get<AtataContext>().Should().NotBeNull().And.Be(AtataContext.Current);

    [Then("Scenario parent AtataContext is a feature context")]
    public void ThenScenarioParentAtataContextIsFeatureContext() =>
        _scenarioContext.Get<AtataContext>().ParentContext!.Test
            .Should().Be(new TestInfo(typeof(AtataContextFeature)));

    [Then("Scenario grandparent AtataContext is a global context")]
    public void ThenScenarioGrandparentAtataContextIsGlobalContext() =>
        _scenarioContext.Get<AtataContext>().ParentContext!.ParentContext
            .Should().NotBeNull().And.Be(AtataContext.Global);
}
