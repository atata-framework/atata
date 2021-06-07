using System;

namespace Atata
{
    /// <summary>
    /// Represents the factory of <see cref="LogEventInfo"/>, which populates the log event with information from <see cref="AtataContext"/>.
    /// </summary>
    public class AtataContextLogEventInfoFactory : ILogEventInfoFactory
    {
        private readonly AtataContext context;

        public AtataContextLogEventInfoFactory(AtataContext context)
        {
            this.context = context.CheckNotNull(nameof(context));
        }

        /// <inheritdoc/>
        public LogEventInfo Create(LogLevel level, string message) =>
            new LogEventInfo
            {
                Level = level,
                Message = message,

                Context = context,
                Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, context.TimeZone),
                BuildStart = context.BuildStartInTimeZone,

                TestName = context.TestName,
                TestNameSanitized = context.TestNameSanitized,
                TestSuiteName = context.TestSuiteName,
                TestSuiteNameSanitized = context.TestSuiteNameSanitized,
                TestStart = context.StartedAt,
                DriverAlias = context.DriverAlias
            };
    }
}
