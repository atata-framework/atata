namespace Atata;

public class CustomDriverAtataContextBuilder : DriverAtataContextBuilder<CustomDriverAtataContextBuilder>
{
    private readonly Func<IWebDriver> _driverFactory;

    public CustomDriverAtataContextBuilder(Func<IWebDriver> driverFactory) =>
        _driverFactory = driverFactory.CheckNotNull(nameof(driverFactory));

    protected override IWebDriver CreateDriver(ILogManager logManager)
    {
        var driver = _driverFactory.Invoke();

        if (driver is not null)
            logManager?.Trace($"Use {GetDriverStringForLog(driver)}");

        return driver;
    }
}
