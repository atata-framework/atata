using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

        public static void SetUp(Func<RemoteWebDriver> driverFactory = null, ILogManager log = null, string testName = null)
        {
            Current = new AtataContext
            {
                TemporarilyPreservedPageObjectList = new List<UIComponent>(),
                Log = log ?? new SimpleLogManager()
            };

            Current.LogTestStart(testName);

            Current.Log.StartSection("Init web driver");
            Current.Driver = driverFactory != null ? driverFactory() : new FirefoxDriver();
            Current.Log.EndSection();
        }

        private void LogTestStart(string testName)
        {
            StringBuilder logMessageBuilder = new StringBuilder("Starting test");

            if (!string.IsNullOrWhiteSpace(testName))
                logMessageBuilder.AppendFormat(": {0}", testName);

            Current.Log.Info(logMessageBuilder.ToString());
        }

        public static void CleanUp()
        {
            if (Current != null)
            {
                Current.Log.Info("Finished test");

                Current.Log = null;
                Current.Driver.Quit();
                Current.CleanUpTemporarilyPreservedPageObjectList();
                UIComponentResolver.CleanUpPageObject(Current.PageObject);
                Current = null;
            }
        }

        internal void CleanUpTemporarilyPreservedPageObjectList()
        {
            UIComponentResolver.CleanUpPageObjects(TemporarilyPreservedPageObjects);
            TemporarilyPreservedPageObjectList.Clear();
        }
    }
}
