namespace Atata.Xunit;

/// <summary>
/// Represents a base class for Atata Xunit test suite/class.
/// providing configuration and initialization for the test <see cref="AtataContext"/>.
/// </summary>
public abstract class AtataTestSuite : AtataFixture
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtataTestSuite"/> class
    /// with the <see cref="AtataContextScope.Test"/> context scope.
    /// </summary>
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

    /// <summary>
    /// Configures the test <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureTestAtataContext(AtataContextBuilder builder)
    {
    }
}
