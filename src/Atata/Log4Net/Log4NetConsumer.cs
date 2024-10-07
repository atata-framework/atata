namespace Atata;

/// <summary>
/// Represents the log consumer for log4net.
/// </summary>
public class Log4NetConsumer : LazyInitializableLogConsumer, INamedLogConsumer
{
    private static readonly Lazy<Dictionary<LogLevel, dynamic>> s_lazyLogLevelsMap = new(CreateLogLevelsMap);

    private static readonly Lazy<dynamic> s_lazyThreadContextProperties = new(GetThreadContextProperties);

    private string _repositoryName;

    private Assembly _repositoryAssembly;

    /// <summary>
    /// Gets or sets the name of the logger repository.
    /// </summary>
    public string RepositoryName
    {
        get => _repositoryName;
        set
        {
            _repositoryName = value;
            _repositoryAssembly = null;
        }
    }

    /// <summary>
    /// Gets or sets the assembly to use to lookup the repository.
    /// </summary>
    public Assembly RepositoryAssembly
    {
        get => _repositoryAssembly;
        set
        {
            _repositoryAssembly = value;
            _repositoryName = null;
        }
    }

    /// <summary>
    /// Gets or sets the name of the logger.
    /// </summary>
    public string LoggerName { get; set; }

    private static Dictionary<LogLevel, dynamic> CreateLogLevelsMap()
    {
        Dictionary<LogLevel, dynamic> logLevelsMap = [];
        Type logLevelType = Type.GetType("log4net.Core.Level,log4net", true);

        foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
        {
            FieldInfo levelField = logLevelType.GetFieldWithThrowOnError(
                level.ToString(),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField);

            logLevelsMap[level] = levelField.GetValue(null);
        }

        return logLevelsMap;
    }

    private static dynamic GetThreadContextProperties() =>
        Type.GetType("log4net.ThreadContext,log4net", true)
            .GetPropertyWithThrowOnError("Properties", BindingFlags.Public | BindingFlags.Static)
            .GetStaticValue();

    private static MethodInfo GetGetLoggerMethod(params Type[] argumentTypes) =>
        Type.GetType("log4net.LogManager,log4net", true)
            .GetMethodWithThrowOnError("GetLogger", BindingFlags.Public | BindingFlags.Static, argumentTypes);

    protected override void OnLog(LogEventInfo eventInfo)
    {
        var properties = s_lazyThreadContextProperties.Value;

        var variables = eventInfo.Session?.Variables ?? eventInfo.Context.Variables;

        foreach (var item in variables)
            properties[item.Key] = item.Value;

        var level = s_lazyLogLevelsMap.Value[eventInfo.Level];

        Logger.Log(null, level, eventInfo.Message, eventInfo.Exception);

        properties.Clear();
    }

    protected override dynamic GetLogger()
    {
        string loggerName = LoggerName ?? GetType().FullName;

        dynamic log = GetLog(loggerName);

        return log.Logger;
    }

    private dynamic GetLog(string loggerName)
    {
        if (RepositoryName != null)
        {
            return GetGetLoggerMethod(typeof(string), typeof(string))
                .InvokeStaticAsLambda<dynamic>(RepositoryName, loggerName);
        }
        else if (RepositoryAssembly != null)
        {
            return GetGetLoggerMethod(typeof(Assembly), typeof(string))
                .InvokeStaticAsLambda<dynamic>(RepositoryAssembly, loggerName);
        }
        else
        {
            return GetGetLoggerMethod(typeof(string))
                .InvokeStaticAsLambda<dynamic>(loggerName);
        }
    }
}
