using System.Collections.Generic;

namespace Atata.Tests
{
    public class StringListLogConsumer : ILogConsumer
    {
        public List<LogEventInfo> Items { get; } = new List<LogEventInfo>();

        public void Log(LogEventInfo eventInfo)
        {
            Items.Add(eventInfo);
        }
    }
}
