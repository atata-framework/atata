using System;

namespace Atata
{
    /// <summary>
    /// Represents the factory of <see cref="LogEventInfo"/>, which populates the log event with information from <see cref="AtataContext"/>.
    /// </summary>
    public class AtataContextLogEventInfoFactory : ILogEventInfoFactory
    {
        private readonly AtataContext _context;

        public AtataContextLogEventInfoFactory(AtataContext context) =>
            _context = context.CheckNotNull(nameof(context));

        /// <inheritdoc/>
        public LogEventInfo Create(LogLevel level, string message) =>
            new LogEventInfo
            {
                Level = level,
                Message = message,
                Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _context.TimeZone),
                Context = _context
            };
    }
}
