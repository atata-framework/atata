using OpenQA.Selenium.Remote;
using System.Threading;

namespace Atata
{
    public class UITest
    {
        protected RemoteWebDriver NativeDriver { get; set; }
        protected ILogManager Logger { get; set; }
        protected PageObjectContext PageObjectContext { get; private set; }

        protected T GoTo<T>() where T : PageObject<T>, new()
        {
            T pageObject = new T();
            return GoTo(pageObject);
        }

        protected T GoTo<T>(T pageObject) where T : PageObject<T>
        {
            PageObjectContext context = CreatePageObjectContext();
            pageObject.Init(context);
            return pageObject;
        }

        protected T GoTo<T>(string url) where T : PageObject<T>, new()
        {
            GoToUrl(url);
            return GoTo<T>();
        }

        protected T GoTo<T>(string url, T pageObject) where T : PageObject<T>
        {
            GoToUrl(url);
            return GoTo<T>(pageObject);
        }

        protected virtual PageObjectContext CreatePageObjectContext()
        {
            return new PageObjectContext(NativeDriver, Logger);
        }

        protected virtual void GoToUrl(string url)
        {
            Logger.Info("Go to URL '{0}'", url);
            NativeDriver.Navigate().GoToUrl(url);
        }

        protected void Wait(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }
    }
}
