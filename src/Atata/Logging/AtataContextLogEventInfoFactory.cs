using System;

namespace Atata
{
    /// <summary>
    /// Represents the factory of <see cref="LogEventInfo"/>, which populates the log event with information from <see cref="AtataContext"/>.
    /// </summary>
    public class AtataContextLogEventInfoFactory : ILogEventInfoFactory
    {
        private readonly AtataContext _context;

        public AtataContextLogEventInfoFactory(AtataContext context)
        {
            _context = context.CheckNotNull(nameof(context));
        }

        /// <inheritdoc/>
        public LogEventInfo Create(LogLevel level, string message) =>
            new LogEventInfo
            {
                Level = level,
                Message = message,

                Context = _context,
                Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _context.TimeZone),
                BuildStart = _context.BuildStartInTimeZone,

                TestName = _context.TestName,
                TestNameSanitized = _context.TestNameSanitized,
                TestSuiteName = _context.TestSuiteName,
                TestSuiteNameSanitized = _context.TestSuiteNameSanitized,
                TestStart = _context.StartedAt,
                DriverAlias = _context.DriverAlias
            };
    }
}
