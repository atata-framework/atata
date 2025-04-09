namespace Atata.NLog;

internal static class NLogAdapter
{
    private static readonly object s_configurationSyncLock = new();

    internal static FileTarget CreateFileTarget(string name, string filePath, string layout) =>
        new(name)
        {
            FileName = Layout.FromString(filePath),
            Layout = Layout.FromString(layout)
        };

    internal static void AddConfigurationRuleForAllLevels(Target target, string loggerNamePattern)
    {
        lock (s_configurationSyncLock)
        {
            var configuration = NLogManager.Configuration;

            if (configuration is null)
            {
                configuration = new LoggingConfiguration();
                NLogManager.Configuration = configuration;
            }

            configuration.AddRuleForAllLevels(target, loggerNamePattern);

            NLogManager.ReconfigExistingLoggers();
        }
    }

    internal static NLogEventInfo CreateLogEventInfo(LogEventInfo eventInfo)
    {
        var otherEventInfo = new NLogEventInfo
        {
            TimeStamp = eventInfo.Timestamp,
            Level = ConvertLogLevel(eventInfo.Level),
            Message = eventInfo.Message,
            Exception = eventInfo.Exception
        };

        var properties = otherEventInfo.Properties;

        foreach (var item in eventInfo.GetProperties())
            properties[item.Key] = item.Value;

        return otherEventInfo;
    }

    private static NLogLevel ConvertLogLevel(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => NLogLevel.Trace,
            LogLevel.Debug => NLogLevel.Debug,
            LogLevel.Info => NLogLevel.Info,
            LogLevel.Warn => NLogLevel.Warn,
            LogLevel.Error => NLogLevel.Error,
            LogLevel.Fatal => NLogLevel.Fatal,
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(level)
        };
}
