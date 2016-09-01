using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class ATContext
    {
        [ThreadStatic]
        private static ATContext current;

        public RemoteWebDriver Driver { get; internal set; }

        public ILogManager Log { get; internal set; }

        public string BaseUrl { get; internal set; }

        public UIComponent PageObject { get; internal set; }

        internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; }

        internal bool IsNavigated { get; set; }

        private DateTime SetUpDateTime { get; set; }
        private DateTime CleanExecutionStartDateTime { get; set; }

        public ReadOnlyCollection<UIComponent> TemporarilyPreservedPageObjects
        {
            get { return TemporarilyPreservedPageObjectList.ToReadOnly(); }
        }

        public static ATContext Current
        {
            get { return current; }
            private set { current = value; }
        }

        public static void SetUp(Func<RemoteWebDriver> driverFactory = null, ILogManager log = null, string testName = null, string baseUrl = null)
        {
            if (baseUrl != null && !Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
                throw new ArgumentException("Invalid URL format \"{0}\".".FormatWith(baseUrl), "baseUrl");

            Current = new ATContext
            {
                TemporarilyPreservedPageObjectList = new List<UIComponent>(),
                Log = log ?? new LogManager().Use(new DebugLogConsumer()),
                BaseUrl = baseUrl,
                SetUpDateTime = DateTime.UtcNow
            };

            Current.LogTestStart(testName);

            Current.Log.Start("Init WebDriver");
            Current.Driver = driverFactory != null ? driverFactory() : new FirefoxDriver();
            Current.Log.EndSection();

            Current.CleanExecutionStartDateTime = DateTime.UtcNow;
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
                TimeSpan cleanTestExecutionTime = DateTime.UtcNow - Current.CleanExecutionStartDateTime;

                Current.Log.Start("Clean-up test context");

                Current.Driver.Quit();
                Current.CleanUpTemporarilyPreservedPageObjectList();

                if (Current.PageObject != null)
                    UIComponentResolver.CleanUpPageObject(Current.PageObject);

                Current.Log.EndSection();

                TimeSpan testExecutionTime = DateTime.UtcNow - Current.SetUpDateTime;
                Current.Log.InfoWithExecutionTimeInBrackets("Finished test", testExecutionTime);
                Current.Log.InfoWithExecutionTime("Сlean test execution time: ", cleanTestExecutionTime);

                Current.Log = null;
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
