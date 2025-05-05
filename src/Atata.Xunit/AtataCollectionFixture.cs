namespace Atata.Xunit;

/// <summary>
/// Represents a base class for Atata Xunit collection fixture,
/// providing configuration and initialization for the collection (test suite group) <see cref="AtataContext"/>.
/// </summary>
public abstract class AtataCollectionFixture : AtataFixture
{
    private readonly string _collectionName;

    /// <summary>
    /// Initializes a new instance of the <see cref="AtataCollectionFixture"/> class
    /// with the <see cref="AtataContextScope.TestSuiteGroup"/> context scope.
    /// </summary>
    /// <param name="collectionName">The name of the collection.</param>
    protected AtataCollectionFixture(string collectionName)
        : base(AtataContextScope.TestSuiteGroup) =>
        _collectionName = collectionName;

    private protected sealed override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        builder.UseTestSuiteType(GetType());
        builder.UseTestSuiteGroupName(_collectionName);

        ConfigureCollectionAtataContext(builder);
    }

    /// <summary>
    /// Configures the collection <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureCollectionAtataContext(AtataContextBuilder builder)
    {
    }
}
