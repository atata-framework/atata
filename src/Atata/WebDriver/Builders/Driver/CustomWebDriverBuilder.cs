﻿namespace Atata;

public class CustomWebDriverBuilder : WebDriverBuilder<CustomWebDriverBuilder>
{
    private readonly Func<IWebDriver> _driverFactory;

    public CustomWebDriverBuilder(Func<IWebDriver> driverFactory) =>
        _driverFactory = driverFactory.CheckNotNull(nameof(driverFactory));

    protected override IWebDriver CreateDriver(ILogManager logManager)
    {
        var driver = _driverFactory.Invoke();

        if (driver is not null)
            logManager?.Trace($"Use {GetDriverStringForLog(driver)}");

        return driver;
    }
}
