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
        ITest xunitTest = TestContext.Current.Test!;
        var testFullName = xunitTest.TestDisplayName;
        var testClassType = GetType();
        var testName = testFullName.Replace(testClassType.FullName!, null).TrimStart('.');
        var output = TestContext.Current.TestOutputHelper!;
        var traits = GetTestTraits(xunitTest);

        builder.UseTestName(testName);
        builder.UseTestSuiteType(testClassType);
        builder.UseTestTraits(traits);

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

    private static List<TestTrait> GetTestTraits(ITestMetadata testMetadata)
    {
        var xunitTraits = testMetadata.Traits;

        if (xunitTraits.Count == 0)
            return [];

        List<TestTrait> traits = new(xunitTraits.Count);

        foreach (var xunitTrait in xunitTraits)
        {
            foreach (string value in xunitTrait.Value)
            {
                traits.Add(new TestTrait(xunitTrait.Key, value));
            }
        }

        return traits;
    }
}
