namespace Atata.Xunit;

/// <summary>
/// Represents a class fixture for Atata Xunit test suites/classes,
/// providing configuration and initialization for the test suite <see cref="AtataContext"/>.
/// </summary>
/// <typeparam name="TClass">The type of the test class.</typeparam>
public class AtataClassFixture<TClass> : AtataFixture
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtataClassFixture{TClass}"/> class.
    /// with the <see cref="AtataContextScope.TestSuite"/> context scope.
    /// </summary>
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
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureSuiteAtataContext(AtataContextBuilder builder)
    {
    }
}
