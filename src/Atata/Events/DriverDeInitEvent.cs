namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> driver is de-initializing.
/// </summary>
public class DriverDeInitEvent
{
    public DriverDeInitEvent(IWebDriver driver) =>
        Driver = driver;

    /// <summary>
    /// Gets the driver.
    /// </summary>
    public IWebDriver Driver { get; }
}
