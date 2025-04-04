namespace Atata;

[Obsolete("Use WebDriverDeInitStartedEvent instead.")] // Obsolete since v4.0.0.
public sealed class DriverDeInitEvent
{
    internal DriverDeInitEvent(IWebDriver driver) =>
        Driver = driver;

    /// <summary>
    /// Gets the driver.
    /// </summary>
    public IWebDriver Driver { get; }
}
