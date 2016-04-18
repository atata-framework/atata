using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atata
{
    public class AtataContext
    {
        [ThreadStatic]
        private static AtataContext current;

        public RemoteWebDriver Driver { get; internal set; }

        public ILogManager Log { get; internal set; }

        public UIComponent PageObject { get; internal set; }

        internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; }

        public ReadOnlyCollection<UIComponent> TemporarilyPreservedPageObjects
        {
            get { return TemporarilyPreservedPageObjectList.ToReadOnly(); }
        }

        public static AtataContext Current
        {
            get { return current; }
            private set { current = value; }
        }

        public static void Init(RemoteWebDriver driver, ILogManager log)
        {
            Current = new AtataContext
            {
                Driver = driver,
                Log = log,
                TemporarilyPreservedPageObjectList = new List<UIComponent>()
            };
        }
    }
}
