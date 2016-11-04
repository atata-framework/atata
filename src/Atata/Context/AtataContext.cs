using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [ThreadStatic]
        private static AtataContext current;

        private bool disposed;

        internal AtataContext()
        {
        }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        public static AtataContext Current
        {
            get { return current; }
            internal set { current = value; }
        }

        /// <summary>
        /// Gets the build start date and time. Contains the same value for all the tests being executed within one build.
        /// </summary>
        public static DateTime? BuildStart { get; private set; }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        public RemoteWebDriver Driver { get; internal set; }

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

        public string BaseUrl { get; internal set; }

        internal List<Action> CleanUpActions { get; set; }

        public UIComponent PageObject { get; internal set; }

        internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; } = new List<UIComponent>();

        internal bool IsNavigated { get; set; }

        internal DateTime CleanExecutionStartDateTime { get; set; }

        public ReadOnlyCollection<UIComponent> TemporarilyPreservedPageObjects
        {
            get { return TemporarilyPreservedPageObjectList.ToReadOnly(); }
        }

        public static AtataContextBuilder Build()
        {
            return new AtataContextBuilder(new AtataBuildingContext());
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

            Log.Start("Clean-up test context");

            if (quitDriver)
                Driver.Quit();

            CleanUpTemporarilyPreservedPageObjectList();

            if (PageObject != null)
                UIComponentResolver.CleanUpPageObject(PageObject);

            Log.EndSection();

            TimeSpan testExecutionTime = DateTime.Now - TestStart;
            Log.InfoWithExecutionTimeInBrackets("Finished test", testExecutionTime);
            Log.InfoWithExecutionTime("Сlean test execution time: ", cleanTestExecutionTime);

            Log = null;

            if (Current == this)
                Current = null;

            disposed = true;
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
                    Log.Error("Clean-up action failure.", e);
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
