using System;

namespace Atata
{
    public class LogSection
    {
        public LogSection(string message = null, LogLevel level = LogLevel.Info)
        {
            Message = message;
            Level = level;
        }

        public string Message { get; protected set; }

        public LogLevel Level { get; protected set; }

        public DateTime StartedAt { get; internal set; }

        public TimeSpan GetDuration()
        {
            return DateTime.Now - StartedAt;
        }
    }
}
