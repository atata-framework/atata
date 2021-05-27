using System;

namespace Atata
{
    /// <summary>
    /// Represents the logging event information raised by Atata framework.
    /// </summary>
    public class LogEventInfo
    {
        internal LogEventInfo()
            : this(AtataContext.Current)
        {
        }

        internal LogEventInfo(AtataContext context)
        {
            Context = context;

            Timestamp = context is null
                ? DateTime.Now
                : TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, context.TimeZone);

            BuildStart = context?.BuildStartInTimeZone ?? AtataContext.BuildStart ?? DateTime.MinValue;
            TestName = context?.TestName;
            TestNameSanitized = context?.TestNameSanitized;
            TestFixtureName = context?.TestFixtureName;
            TestFixtureNameSanitized = context?.TestFixtureNameSanitized;
            TestStart = context?.StartedAt ?? DateTime.MinValue;
            DriverAlias = context?.DriverAlias;
        }

        /// <summary>
        /// Gets the context of the logging event.
        /// Can be <see langword="null"/>.
        /// </summary>
        public AtataContext Context { get; }

        /// <summary>
        /// Gets the timestamp of the logging event.
        /// </summary>
        public DateTime Timestamp { get; }

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
        public DateTime BuildStart { get; }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string TestName { get; }

        /// <summary>
        /// Gets the name of the test sanitized for file path/name.
        /// </summary>
        public string TestNameSanitized { get; }

        /// <summary>
        /// Gets the name of the test fixture.
        /// </summary>
        public string TestFixtureName { get; }

        /// <summary>
        /// Gets the name of the test fixture sanitized for file path/name.
        /// </summary>
        public string TestFixtureNameSanitized { get; }

        /// <summary>
        /// Gets the test start date and time.
        /// </summary>
        public DateTime TestStart { get; }

        /// <summary>
        /// Gets the alias of the driver.
        /// </summary>
        public string DriverAlias { get; }

        /// <summary>
        /// Gets the nesting level.
        /// </summary>
        public int NestingLevel { get; internal set; }
    }
}
