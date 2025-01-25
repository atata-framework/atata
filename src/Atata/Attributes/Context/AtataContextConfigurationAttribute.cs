#nullable enable

namespace Atata;

/// <summary>
/// Represents a base attribute class that configures <see cref="AtataContext"/> via <see cref="AtataContextBuilder"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public abstract class AtataContextConfigurationAttribute : Attribute
{
    /// <summary>
    /// Configures the targeted <see cref="AtataContext"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    public abstract void ConfigureAtataContext(AtataContextBuilder builder);
}
