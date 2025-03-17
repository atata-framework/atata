namespace Atata.MSTest;

public static class TestContextExtensions
{
    private const string AtataContextPropertyKey = nameof(AtataContext);

    public static void SetAtataContext(this TestContext testContext, AtataContext atataContext) =>
        testContext.Properties[AtataContextPropertyKey] = atataContext;

    public static void RemoveAtataContext(this TestContext testContext) =>
        testContext.Properties.Remove(AtataContextPropertyKey);

    public static AtataContext GetAtataContext(this TestContext testContext) =>
        testContext.GetAtataContextOrNull()
            ?? throw new AtataContextNotFoundException(
                $"Failed to find {nameof(AtataContext)} instance in {nameof(TestContext)}.");

    public static AtataContext? GetAtataContextOrNull(this TestContext testContext) =>
        testContext.Properties[AtataContextPropertyKey] as AtataContext;
}
