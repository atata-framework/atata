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
    public IScreenshotStrategy<WebDriverSession> Strategy { get; set; } =
        WebDriverViewportScreenshotStrategy.Instance;

    /// <summary>
    /// Gets or sets the page screenshot file name template.
    /// The file name is relative to Artifacts path.
    /// The default value is <c>"{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}"</c>.
    /// </summary>
    public string FileNameTemplate { get; set; } =
        "{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}";

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
            clone.Strategy = (IScreenshotStrategy<WebDriverSession>)cloneableScreenshotStrategy.Clone();

        return clone;
    }
}
