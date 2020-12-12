using System;

namespace Atata
{
    /// <summary>
    /// Represents the logging event information raised by Atata framework.
    /// </summary>
    public class LogEventInfo
    {
        internal LogEventInfo()
        {
            Timestamp = DateTime.Now;
            BuildStart = AtataContext.BuildStart ?? DateTime.MinValue;
            TestName = AtataContext.Current?.TestName;
            TestNameSanitized = AtataContext.Current?.TestNameSanitized;
            TestStart = AtataContext.Current?.StartedAt ?? DateTime.MinValue;
            DriverAlias = AtataContext.Current?.DriverAlias;
        }

        /// <summary>
        /// Gets the timestamp of the logging event.
        /// </summary>
        public DateTime Timestamp { get; private set; }

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

        /// <summary>
        /// Gets the build start date and time.
        /// Contains the same value for all the tests being executed within one build.
        /// </summary>
        public DateTime BuildStart { get; private set; }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string TestName { get; private set; }

        /// <summary>
        /// Gets the name of the test sanitized for file path/name.
        /// </summary>
        public string TestNameSanitized { get; private set; }

        /// <summary>
        /// Gets the test start date and time.
        /// </summary>
        public DateTime TestStart { get; private set; }

        /// <summary>
        /// Gets the alias of the driver.
        /// </summary>
        public string DriverAlias { get; private set; }

        /// <summary>
        /// Gets the nesting level.
        /// </summary>
        public int NestingLevel { get; internal set; }
    }
}
