using OpenQA.Selenium.Remote;
using System.Threading;

namespace Atata
{
    public class UITest
    {
        protected RemoteWebDriver Driver { get; set; }
        protected ILogManager Log { get; set; }

        protected void Wait(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }
    }
}
