namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="WebDriverSession.Driver"/> is initialized.
/// </summary>
public sealed class WebDriverInitCompletedEvent
{
    internal WebDriverInitCompletedEvent(IWebDriver driver) =>
        Driver = driver;

    /// <summary>
    /// Gets the driver.
    /// </summary>
    public IWebDriver Driver { get; }
}
