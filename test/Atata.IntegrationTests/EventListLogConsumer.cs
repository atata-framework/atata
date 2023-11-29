namespace Atata.IntegrationTests;

public class EventListLogConsumer : ILogConsumer
{
    public List<LogEventInfo> Items { get; } = [];

    public void Log(LogEventInfo eventInfo) =>
        Items.Add(eventInfo);
}
