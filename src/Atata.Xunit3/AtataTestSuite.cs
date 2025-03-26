namespace Atata.Xunit;

public abstract class AtataTestSuite : AtataFixture
{
    protected AtataTestSuite()
        : base(AtataContextScope.Test)
    {
    }

    private protected sealed override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        var testFullName = TestContext.Current.Test!.TestDisplayName;
        var testClassType = GetType();
        var testName = testFullName.Replace(testClassType.FullName!, null).TrimStart('.');
        var output = TestContext.Current.TestOutputHelper!;

        builder.UseTestName(testName);
        builder.UseTestSuiteType(testClassType);

        if (CollectionResolver.TryResolveCollectionName(testClassType, out var collectionName))
            builder.UseTestSuiteGroupName(collectionName);

        builder.LogConsumers.Add(new TextOutputLogConsumer(output.WriteLine));

        ConfigureTestAtataContext(builder);
    }

    protected virtual void ConfigureTestAtataContext(AtataContextBuilder builder)
    {
    }
}
