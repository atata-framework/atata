#nullable enable

namespace Atata;

/// <summary>
/// Represents the <see cref="AtataContext"/> metadata of a test.
/// </summary>
public sealed class TestAtataContextMetadata
{
    private readonly object[] _attributes;

    private TestAtataContextMetadata(object[] attributes) =>
        _attributes = attributes;

    /// <summary>
    /// Gets the <see cref="TestAtataContextMetadata"/> for the specified <paramref name="testMethod"/>.
    /// </summary>
    /// <param name="testMethod">The test method.</param>
    /// <returns>A new <see cref="TestAtataContextMetadata"/> instance.</returns>
    public static TestAtataContextMetadata GetForMethod(MethodInfo testMethod)
    {
        testMethod.CheckNotNull(nameof(testMethod));

        object[] allAttributes = testMethod.GetCustomAttributes(true);
        return new(allAttributes);
    }

    /// <summary>
    /// Applies the metadata to the specified <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The builder to apply the metadata to.</param>
    public void ApplyToTestBuilder(AtataContextBuilder builder)
    {
        foreach (object attribute in _attributes)
        {
            if (attribute is AtataContextConfigurationAttribute configurationAttribute)
                configurationAttribute.ConfigureAtataContext(builder);
        }
    }
}
