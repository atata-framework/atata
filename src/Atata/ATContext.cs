using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata
{
    /// <summary>
    /// Represents the Atata context, the entry point for the test set-up.
    /// </summary>
    public class ATContext
    {
        private static readonly object LockObject = new object();

        [ThreadStatic]
        private static ATContext current;

        /// <summary>
        /// Gets the build start date and time.
        /// </summary>
        /// <value>
        /// The build start. Contains the same value for all the tests being executed within one build.
        /// </value>
        public static DateTime? BuildStart { get; private set; }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        /// <value>
        /// The driver.
        /// </value>
        public RemoteWebDriver Driver { get; private set; }

        /// <summary>
        /// Gets the log manager.
        /// </summary>
        /// <value>
        /// The instance of the log manager.
        /// </value>
        public ILogManager Log { get; private set; }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        /// <value>
        /// The name of the test.
        /// </value>
        public string TestName { get; private set; }

        /// <summary>
        /// Gets the test start date and time.
        /// </summary>
        /// <value>
        /// The test start.
        /// </value>
        public DateTime TestStart { get; private set; }

        public string BaseUrl { get; private set; }

        public UIComponent PageObject { get; internal set; }

        internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; }

        internal bool IsNavigated { get; set; }

        private DateTime CleanExecutionStartDateTime { get; set; }

        public ReadOnlyCollection<UIComponent> TemporarilyPreservedPageObjects
        {
            get { return TemporarilyPreservedPageObjectList.ToReadOnly(); }
        }

        /// <summary>
        /// Gets the current test context.
        /// </summary>
        /// <value>
        /// The test context.
        /// </value>
        public static ATContext Current
        {
            get { return current; }
            private set { current = value; }
        }

        public static void SetUp(Func<RemoteWebDriver> driverFactory = null, ILogManager log = null, string testName = null, string baseUrl = null)
        {
            if (baseUrl != null && !Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
                throw new ArgumentException("Invalid URL format \"{0}\".".FormatWith(baseUrl), nameof(baseUrl));

            InitGlobalVariables();

            Current = new ATContext
            {
                TemporarilyPreservedPageObjectList = new List<UIComponent>(),
                Log = log ?? new LogManager().Use(new DebugLogConsumer()),
                TestName = testName,
                TestStart = DateTime.Now,
                BaseUrl = baseUrl
            };

            Current.LogTestStart();

            Current.Log.Start("Init WebDriver");
            Current.Driver = driverFactory != null ? driverFactory() : new FirefoxDriver();
            Current.Log.EndSection();

            Current.CleanExecutionStartDateTime = DateTime.Now;
        }

        private static void InitGlobalVariables()
        {
            if (BuildStart == null)
            {
                lock (LockObject)
                {
                    if (BuildStart == null)
                    {
                        BuildStart = DateTime.Now;
                    }
                }
            }
        }

        private void LogTestStart()
        {
            StringBuilder logMessageBuilder = new StringBuilder("Starting test");

            if (!string.IsNullOrWhiteSpace(TestName))
                logMessageBuilder.Append($": {TestName}");

            Current.Log.Info(logMessageBuilder.ToString());
        }

        /// <summary>
        /// Cleans up current test context.
        /// </summary>
        public static void CleanUp()
        {
            if (Current != null)
            {
                TimeSpan cleanTestExecutionTime = DateTime.Now - Current.CleanExecutionStartDateTime;

                Current.Log.Start("Clean-up test context");

                Current.Driver.Quit();
                Current.CleanUpTemporarilyPreservedPageObjectList();

                if (Current.PageObject != null)
                    UIComponentResolver.CleanUpPageObject(Current.PageObject);

                Current.Log.EndSection();

                TimeSpan testExecutionTime = DateTime.Now - Current.TestStart;
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
