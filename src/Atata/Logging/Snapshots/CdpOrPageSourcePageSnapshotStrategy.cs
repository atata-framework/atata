using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace Atata;

/// <summary>
/// Represents the strategy that takes a page snapshot using CDP,
/// or when CDP is not available takes <see cref="IWebDriver.PageSource"/>.
/// </summary>
public sealed class CdpOrPageSourcePageSnapshotStrategy : IPageSnapshotStrategy
{
    private static readonly ConcurrentDictionary<string, bool> s_driverAliasSupportsCdpMap = new();

    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static CdpOrPageSourcePageSnapshotStrategy Instance { get; } =
        new CdpOrPageSourcePageSnapshotStrategy();

    /// <inheritdoc/>
    public FileContentWithExtension TakeSnapshot(WebDriverSession session)
    {
        string driverAlias = session.DriverAlias;

        if (string.IsNullOrEmpty(driverAlias))
            driverAlias = session.Driver.GetType().Name;

        if (!s_driverAliasSupportsCdpMap.TryGetValue(driverAlias, out bool isCdpSupported))
        {
            var driver = session.Driver;

            isCdpSupported = driver.Is<ChromeDriver>()
                || driver.Is<EdgeDriver>()
                || driver.Is<RemoteWebDriver>();

            s_driverAliasSupportsCdpMap[driverAlias] = isCdpSupported;
        }

        if (isCdpSupported)
        {
            try
            {
                return CdpPageSnapshotStrategy.Instance.TakeSnapshot(session);
            }
            catch (Exception exception)
            {
                s_driverAliasSupportsCdpMap[driverAlias] = false;

                session.Log.Warn(exception, "Failed to take CDP snapshot. PageSource snapshot will be taken.");
            }
        }

        return PageSourcePageSnapshotStrategy.Instance.TakeSnapshot(session);
    }
}
