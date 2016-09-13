namespace Atata
{
    public class LogConsumerInfo
    {
        public LogConsumerInfo(ILogConsumer consumer, LogLevel minLevel = LogLevel.Trace, bool logSectionFinish = true)
        {
            Consumer = consumer;
            LogSectionFinish = logSectionFinish;
            MinLevel = minLevel;
        }

        public ILogConsumer Consumer { get; private set; }

        public LogLevel MinLevel { get; internal set; }

        public bool LogSectionFinish { get; internal set; }
    }
}
