using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents an event that occurs when <see cref="AtataContext"/> driver is initializing.
    /// </summary>
    public class DriverInitEvent
    {
        public DriverInitEvent(IWebDriver driver)
        {
            Driver = driver;
        }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        public IWebDriver Driver { get; }
    }
}
