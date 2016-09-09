using OpenQA.Selenium.Remote;

namespace Atata
{
    public class PageObjectContext
    {
        public PageObjectContext(RemoteWebDriver driver, ILogManager logger)
        {
            Driver = driver;
            Logger = logger;
        }

        public RemoteWebDriver Driver { get; set; }

        public ILogManager Logger { get; set; }
    }
}
