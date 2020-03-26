namespace Atata
{
    /// <summary>
    /// Represents the base class for log consumer that needs to be initialized in a lazy way.
    /// </summary>
    public abstract class LazyInitializableLogConsumer : ILogConsumer
    {
        private readonly object loggerInitializationLock = new object();

        private bool isInitialized;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected dynamic Logger { get; private set; }

        public void Log(LogEventInfo eventInfo)
        {
            EnsureLoggerIsInitialized();

            if (Logger != null)
                OnLog(eventInfo);
        }

        private void EnsureLoggerIsInitialized()
        {
            if (!isInitialized)
            {
                lock (loggerInitializationLock)
                {
                    if (!isInitialized)
                    {
                        try
                        {
                            Logger = GetLogger();
                        }
                        finally
                        {
                            isInitialized = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Logs the specified event information.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        protected abstract void OnLog(LogEventInfo eventInfo);

        /// <summary>
        /// Gets a logger to set to <see cref="Logger"/> property and later use for logging.
        /// </summary>
        /// <returns>A logger instance.</returns>
        protected abstract dynamic GetLogger();
    }
}
