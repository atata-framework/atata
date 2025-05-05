namespace Atata.Xunit;

/// <summary>
/// Represents a class fixture for Atata Xunit test suites/classes,
/// providing configuration and initialization for the test suite <see cref="AtataContext"/>.
/// </summary>
/// <typeparam name="TClass">The type of the test class.</typeparam>
public class AtataClassFixture<TClass> : AtataFixture
{
    public AtataClassFixture()
        : base(AtataContextScope.TestSuite)
    {
    }

    private protected sealed override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        Type testClassType = typeof(TClass);
        builder.UseTestSuiteType(testClassType);

        if (CollectionResolver.TryResolveCollectionName(testClassType, out var collectionName))
            builder.UseTestSuiteGroupName(collectionName);

        ConfigureSuiteAtataContext(builder);
    }

    /// <summary>
    /// Configures the test suite <see cref="AtataContext"/>.
    /// This method can be overridden in derived classes to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureSuiteAtataContext(AtataContextBuilder builder)
    {
    }
}
