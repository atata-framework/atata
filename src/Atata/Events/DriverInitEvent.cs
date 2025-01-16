#nullable enable

namespace Atata;

[Obsolete("Use WebDriverInitCompletedEvent instead.")] // Obsolete since v4.0.0.
public sealed class DriverInitEvent
{
    internal DriverInitEvent(IWebDriver driver) =>
        Driver = driver;

    /// <summary>
    /// Gets the driver.
    /// </summary>
    public IWebDriver Driver { get; }
}
