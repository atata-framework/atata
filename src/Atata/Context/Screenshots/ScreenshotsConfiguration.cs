namespace Atata;

/// <summary>
/// Represents the configuration of page screenshots functionality.
/// </summary>
public sealed class ScreenshotsConfiguration : ICloneable
{
    /// <summary>
    /// Gets or sets the strategy for a page screenshot taking.
    /// The default value is an instance of <see cref="WebDriverViewportScreenshotStrategy"/>.
    /// </summary>
    public IScreenshotStrategy Strategy { get; set; } = WebDriverViewportScreenshotStrategy.Instance;

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    internal ScreenshotsConfiguration Clone()
    {
        ScreenshotsConfiguration clone = (ScreenshotsConfiguration)MemberwiseClone();

        if (Strategy is ICloneable cloneableScreenshotStrategy)
            clone.Strategy = (IScreenshotStrategy)cloneableScreenshotStrategy.Clone();

        return clone;
    }
}
