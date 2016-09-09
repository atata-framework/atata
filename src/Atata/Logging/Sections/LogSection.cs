using System;

namespace Atata
{
    public class LogSection
    {
        public LogSection(string message = null)
        {
            Message = message;
            Level = LogLevel.Info;
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
