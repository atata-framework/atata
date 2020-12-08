using System;

namespace Atata
{
    /// <summary>
    /// Represents the log section.
    /// </summary>
    public class LogSection
    {
        private object result;

        public LogSection(string message = null, LogLevel level = LogLevel.Info)
        {
            Message = message;
            Level = level;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public LogLevel Level { get; protected set; }

        /// <summary>
        /// Gets the date/time of section start.
        /// </summary>
        public DateTime StartedAt { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the result is set.
        /// </summary>
        public bool IsResultSet { get; private set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public object Result
        {
            get => result;
            set
            {
                result = value;
                IsResultSet = true;
            }
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }

        public TimeSpan GetDuration()
        {
            return DateTime.Now - StartedAt;
        }
    }
}
