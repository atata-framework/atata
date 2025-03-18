namespace Atata.Xunit;

public class AtataClassFixture<TClass> : AtataFixture
{
    public AtataClassFixture()
        : base(AtataContextScope.TestSuite)
    {
    }

    protected override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        Type testClassType = typeof(TClass);
        builder.UseTestSuiteType(testClassType);

        if (CollectionResolver.TryResolveCollectionName(testClassType, out var collectionName))
            builder.UseTestSuiteGroupName(collectionName);
    }
}
