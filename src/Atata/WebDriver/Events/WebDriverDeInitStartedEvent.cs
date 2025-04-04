namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="WebDriverSession.Driver"/> is started to deinitialize.
/// </summary>
public sealed class WebDriverDeInitStartedEvent
{
    internal WebDriverDeInitStartedEvent(IWebDriver driver) =>
        Driver = driver;

    /// <summary>
    /// Gets the driver.
    /// </summary>
    public IWebDriver Driver { get; }
}
