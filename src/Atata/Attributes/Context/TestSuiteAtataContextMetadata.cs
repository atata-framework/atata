namespace Atata;

/// <summary>
/// Represents the <see cref="AtataContext"/> metadata of a test suite.
/// </summary>
public sealed class TestSuiteAtataContextMetadata
{
    private readonly object[] _attributes;

    private TestSuiteAtataContextMetadata(object[] attributes) =>
        _attributes = attributes;

    /// <summary>
    /// Gets the <see cref="TestSuiteAtataContextMetadata"/> for the specified <paramref name="testSuiteType"/>.
    /// </summary>
    /// <param name="testSuiteType">The test suite type.</param>
    /// <returns>A new <see cref="TestAtataContextMetadata"/> instance.</returns>
    public static TestSuiteAtataContextMetadata GetForType(Type testSuiteType)
    {
        testSuiteType.CheckNotNull(nameof(testSuiteType));

        object[] allAttributes = testSuiteType.GetCustomAttributes(true);
        return new(allAttributes);
    }

    /// <summary>
    /// Applies the metadata to the specified test suite <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The builder to apply the metadata to.</param>
    public void ApplyToTestSuiteBuilder(AtataContextBuilder builder)
    {
        foreach (object attribute in _attributes)
        {
            if (attribute is AtataContextConfigurationAttribute configurationAttribute)
                configurationAttribute.ConfigureAtataContext(builder);
        }
    }

    /// <summary>
    /// Applies the metadata to the specified test <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The builder to apply the metadata to.</param>
    public void ApplyToTestBuilder(AtataContextBuilder builder)
    {
        foreach (object attribute in _attributes)
        {
            if (attribute is TestSuiteAtataContextConfigurationAttribute configurationAttribute)
                configurationAttribute.ConfigureTestAtataContext(builder);
        }
    }
}
