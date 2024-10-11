namespace Atata.IntegrationTests;

public sealed class FakeLogConsumer : ILogConsumer
{
    private readonly List<LogEventInfo> _records = [];

    public LogEventInfo LatestRecord
    {
        get
        {
            lock (_records)
            {
                if (_records.Count == 0)
                {
                    throw new InvalidOperationException("No records logged.");
                }

                return _records[^1];
            }
        }
    }

    void ILogConsumer.Log(LogEventInfo eventInfo)
    {
        lock (_records)
        {
            _records.Add(eventInfo);
        }
    }

    public LogEventInfo[] GetSnapshot()
    {
        lock (_records)
        {
            return [.. _records];
        }
    }

    public LogEventInfo[] GetSnapshot(int countFromEnd) =>
        GetSnapshot(LogLevel.Trace, countFromEnd);

    public LogEventInfo[] GetSnapshot(LogLevel minLogLevel, int countFromEnd)
    {
        lock (_records)
        {
            return _records.AsEnumerable()
                .Reverse()
                .Where(x => x.Level >= minLogLevel)
                .Take(countFromEnd)
                .Reverse()
                .ToArray();
        }
    }

    public LogEventInfo[] GetSnapshotOfLevel(LogLevel logLevel)
    {
        lock (_records)
        {
            return _records.AsEnumerable()
                .Reverse()
                .Where(x => x.Level == logLevel)
                .Reverse()
                .ToArray();
        }
    }

    public string[] GetMessagesSnapshot() =>
        GetSnapshot()
            .Select(x => x.Message)
            .ToArray();

    public string[] GetMessagesSnapshot(int countFromEnd) =>
        GetSnapshot(countFromEnd)
            .Select(x => x.Message)
            .ToArray();

    public string[] GetMessagesSnapshot(LogLevel minLogLevel, int countFromEnd) =>
        GetSnapshot(minLogLevel, countFromEnd)
            .Select(x => x.Message)
            .ToArray();

    public string[] GetNestingTextsWithMessagesSnapshot() =>
        GetSnapshot()
            .Select(x => x.NestingText + x.Message)
            .ToArray();

    public string[] GetNestingTextsWithMessagesSnapshot(int countFromEnd) =>
        GetSnapshot(countFromEnd)
            .Select(x => x.NestingText + x.Message)
            .ToArray();

    public string[] GetNestingTextsWithMessagesSnapshot(LogLevel minLogLevel, int countFromEnd) =>
        GetSnapshot(minLogLevel, countFromEnd)
            .Select(x => x.NestingText + x.Message)
            .ToArray();
}
