namespace Atata;

/// <summary>
/// Represents a custom web driver builder that allows creating a web driver instance using a specified factory method.
/// </summary>
public class CustomWebDriverBuilder : WebDriverBuilder<CustomWebDriverBuilder>
{
    private readonly Func<IWebDriver> _driverFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomWebDriverBuilder"/> class.
    /// </summary>
    /// <param name="driverFactory">The factory method to create an <see cref="IWebDriver"/> instance.</param>
    public CustomWebDriverBuilder(Func<IWebDriver> driverFactory)
    {
        Guard.ThrowIfNull(driverFactory);

        _driverFactory = driverFactory;
    }

    /// <inheritdoc/>
    protected override IWebDriver CreateDriver(ILogManager logManager)
    {
        var driver = _driverFactory.Invoke();

        if (driver is not null)
            logManager?.Trace($"Use {GetDriverStringForLog(driver)}");

        return driver!;
    }
}
