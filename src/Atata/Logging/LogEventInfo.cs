using System;

namespace Atata
{
    /// <summary>
    /// Represents the logging event.
    /// </summary>
    public class LogEventInfo
    {
        /// <summary>
        /// Gets the timestamp of the logging event.
        /// </summary>
        public DateTime Timestamp { get; internal set; }

        /// <summary>
        /// Gets the level of the logging event.
        /// </summary>
        public LogLevel Level { get; internal set; }

        /// <summary>
        /// Gets the log message.
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// Gets the exception information.
        /// </summary>
        public Exception Exception { get; internal set; }
    }
}
