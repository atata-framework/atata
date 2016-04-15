using OpenQA.Selenium.Remote;
using System;

namespace Atata
{
    public class AtataContext
    {
        [ThreadStatic]
        private static AtataContext current;

        public UIComponent PageObject { get; internal set; }
        public RemoteWebDriver Driver { get; internal set; }
        public ILogManager Log { get; internal set; }

        public static AtataContext Current
        {
            get { return current; }
            private set { current = value; }
        }

        public static void Init(RemoteWebDriver driver, ILogManager log)
        {
            Current = new AtataContext { Driver = driver, Log = log };
        }
    }
}
