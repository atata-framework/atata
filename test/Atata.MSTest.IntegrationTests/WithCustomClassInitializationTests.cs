namespace Atata.MSTest.IntegrationTests;

[TestClass]
[SetVariable("class-attribute-variable", true)]
public sealed class WithCustomClassInitializationTests : AtataTestSuite
{
    [ConfiguresSuiteAtataContext]
    public static void ConfigureSuiteAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable("custom-class-variable-1", true);

    [ClassInitialize]
    public static void SetUpTestSuite(TestContext testContext) =>
        AtataContext.Current!.Variables["custom-class-variable-2"] = true;

    [TestMethod]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [TestMethod]
    [SetVariable("method-attribute-variable", true)]
    public void Context_Variables()
    {
        Context.Variables["custom-class-variable-1"].Should().Be(true);
        Context.Variables["custom-class-variable-2"].Should().Be(true);
        Context.Variables["class-attribute-variable"].Should().Be(true);
        Context.Variables["method-attribute-variable"].Should().Be(true);
    }
}
