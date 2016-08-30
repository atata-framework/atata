using System;

namespace Atata
{
    /// <summary>
    /// Represents the logging event.
    /// </summary>
    public class LogEventInfo
    {
        public LogEventInfo()
        {
            Timestamp = DateTime.Now;
        }

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

        /// <summary>
        /// Gets the section start information.
        /// </summary>
        public LogSection SectionStart { get; internal set; }

        /// <summary>
        /// Gets the section end information.
        /// </summary>
        public LogSection SectionEnd { get; internal set; }
    }
}
