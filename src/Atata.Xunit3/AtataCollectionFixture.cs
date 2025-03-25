namespace Atata.Xunit;

public abstract class AtataCollectionFixture : AtataFixture
{
    private readonly string _collectionName;

    protected AtataCollectionFixture(string collectionName)
        : base(AtataContextScope.TestSuiteGroup) =>
        _collectionName = collectionName;

    private protected sealed override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        builder.UseTestSuiteType(GetType());
        builder.UseTestSuiteGroupName(_collectionName);

        ConfigureCollectionAtataContext(builder);
    }

    protected virtual void ConfigureCollectionAtataContext(AtataContextBuilder builder)
    {
    }
}
