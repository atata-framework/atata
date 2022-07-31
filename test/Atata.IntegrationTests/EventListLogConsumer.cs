using System.Collections.Generic;

namespace Atata.IntegrationTests
{
    public class EventListLogConsumer : ILogConsumer
    {
        public List<LogEventInfo> Items { get; } = new List<LogEventInfo>();

        public void Log(LogEventInfo eventInfo)
        {
            Items.Add(eventInfo);
        }
    }
}
