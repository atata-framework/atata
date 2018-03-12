using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using OpenQA.Selenium.Remote;

namespace Atata
{
    /// <summary>
    /// Represents the Atata context, the entry point for the test set-up.
    /// </summary>
    public sealed class AtataContext : IDisposable
    {
        private static readonly object LockObject = new object();

        private static bool isThreadStatic = true;

        [ThreadStatic]
        private static AtataContext currentThreadStaticContext;

        private static AtataContext currentStaticContext;

        private bool disposed;

        internal AtataContext()
        {
        }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        public static AtataContext Current
        {
            get => isThreadStatic ? currentThreadStaticContext : currentStaticContext;
            internal set
            {
                if (isThreadStatic)
                    currentThreadStaticContext = value;
                else
                    currentStaticContext = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Current"/> property use thread-static approach (value unique for each thread). The default value is <c>true</c>.
        /// </summary>
        public static bool IsThreadStatic
        {
            get => isThreadStatic;
            set
            {
                isThreadStatic = value;
                RetrySettings.IsThreadStatic = value;
            }
        }

        /// <summary>
        /// Gets the global configuration.
        /// </summary>
        public static AtataContextBuilder GlobalConfiguration { get; } = new AtataContextBuilder(new AtataBuildingContext());

        /// <summary>
        /// Gets the build start date and time. Contains the same value for all the tests being executed within one build.
        /// </summary>
        public static DateTime? BuildStart { get; private set; }

        internal IDriverFactory DriverFactory { get; set; }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        public RemoteWebDriver Driver { get; internal set; }

        /// <summary>
        /// Gets the driver alias.
        /// </summary>
        public string DriverAlias { get; internal set; }

        /// <summary>
        /// Gets the instance of the log manager.
        /// </summary>
        public ILogManager Log { get; internal set; }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string TestName { get; internal set; }

        /// <summary>
        /// Gets the test start date and time.
        /// </summary>
        public DateTime TestStart { get; private set; } = DateTime.Now;

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        public string BaseUrl { get; internal set; }

        /// <summary>
        /// Gets the base retry timeout. The default value is 5 seconds.
        /// </summary>
        [Obsolete("Use BaseRetryTimeout instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryTimeout => BaseRetryTimeout;

        /// <summary>
        /// Gets the base retry interval. The default value is 500 milliseconds.
        /// </summary>
        [Obsolete("Use BaseRetryInterval instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryInterval => BaseRetryInterval;

        /// <summary>
        /// Gets the base retry timeout. The default value is 5 seconds.
        /// </summary>
        public TimeSpan BaseRetryTimeout { get; internal set; }

        /// <summary>
        /// Gets the base retry interval. The default value is 500 milliseconds.
        /// </summary>
        public TimeSpan BaseRetryInterval { get; internal set; }

        /// <summary>
        /// Gets the waiting timeout. The default value is 5 seconds.
        /// </summary>
        public TimeSpan WaitingTimeout { get; internal set; }

        /// <summary>
        /// Gets the waiting retry interval. The default value is 500 milliseconds.
        /// </summary>
        public TimeSpan WaitingRetryInterval { get; internal set; }

        /// <summary>
        /// Gets the culture. The default value is <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        public CultureInfo Culture { get; internal set; }

        /// <summary>
        /// Gets the type of the assertion exception. The default value is typeof(Atata.AssertionException).
        /// </summary>
        public Type AssertionExceptionType { get; internal set; }

        internal List<Action> CleanUpActions { get; set; }

        public UIComponent PageObject { get; internal set; }

        internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; } = new List<UIComponent>();

        internal bool IsNavigated { get; set; }

        internal DateTime CleanExecutionStartDateTime { get; set; }

        public ReadOnlyCollection<UIComponent> TemporarilyPreservedPageObjects
        {
            get { return TemporarilyPreservedPageObjectList.ToReadOnly(); }
        }

        internal UIComponentScopeCache UIComponentScopeCache { get; } = new UIComponentScopeCache();

        [Obsolete("Use Configure() instead.")] // Obsolete since v0.14.0.
        public static AtataContextBuilder Build()
        {
            return Configure();
        }

        /// <summary>
        /// Creates <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration. Sets the value to <see cref="AtataContextBuilder.BuildingContext"/> copied from <see cref="GlobalConfiguration"/>.
        /// </summary>
        /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder Configure()
        {
            AtataBuildingContext buildingContext = GlobalConfiguration.BuildingContext.Clone();
            return new AtataContextBuilder(buildingContext);
        }

        internal static void InitGlobalVariables()
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

        internal void LogTestStart()
        {
            StringBuilder logMessageBuilder = new StringBuilder("Starting test");

            if (!string.IsNullOrWhiteSpace(TestName))
                logMessageBuilder.Append($": {TestName}");

            Current.Log.Info(logMessageBuilder.ToString());
        }

        /// <summary>
        /// Cleans up the test context.
        /// </summary>
        /// <param name="quitDriver">if set to <c>true</c> quits WebDriver.</param>
        public void CleanUp(bool quitDriver = true)
        {
            if (disposed)
                return;

            ExecuteCleanUpActions();

            TimeSpan cleanTestExecutionTime = DateTime.Now - CleanExecutionStartDateTime;

            Log.Start("Clean up test context");

            CleanUpTemporarilyPreservedPageObjectList();

            if (PageObject != null)
                UIComponentResolver.CleanUpPageObject(PageObject);

            UIComponentScopeCache.Clear();

            if (quitDriver)
                Driver.Dispose();

            Log.EndSection();

            TimeSpan testExecutionTime = DateTime.Now - TestStart;
            Log.InfoWithExecutionTimeInBrackets("Finished test", testExecutionTime);
            Log.InfoWithExecutionTime("Pure test execution time:", cleanTestExecutionTime);

            Log = null;

            if (Current == this)
                Current = null;

            disposed = true;
        }

        /// <summary>
        /// Restarts the driver.
        /// </summary>
        public void RestartDriver()
        {
            Log.Start("Restart driver");

            CleanUpTemporarilyPreservedPageObjectList();

            if (PageObject != null)
            {
                UIComponentResolver.CleanUpPageObject(PageObject);
                PageObject = null;
            }

            Driver.Dispose();

            Driver = DriverFactory.Create();

            Log.EndSection();
        }

        internal void CleanUpTemporarilyPreservedPageObjectList()
        {
            UIComponentResolver.CleanUpPageObjects(TemporarilyPreservedPageObjects);
            TemporarilyPreservedPageObjectList.Clear();
        }

        private void ExecuteCleanUpActions()
        {
            foreach (Action action in CleanUpActions)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Log.Error("Clean up action failure.", e);
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CleanUp();
        }
    }
}
