namespace Atata;

/// <summary>
/// Represents a base attribute class that configures test suite <see cref="AtataContext"/> via <see cref="AtataContextBuilder"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class TestSuiteAtataContextConfigurationAttribute : AtataContextConfigurationAttribute
{
    /// <summary>
    /// Configures the <see cref="AtataContext"/> of each test.
    /// </summary>
    /// <param name="builder">The builder.</param>
    public virtual void ConfigureTestAtataContext(AtataContextBuilder builder)
    {
    }
}
