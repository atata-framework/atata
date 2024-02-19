namespace Atata;

/// <summary>
/// Represents the log consumer that writes log to file using NLog.
/// </summary>
public class NLogFileConsumer : LazyInitializableLogConsumer, ICloneable
{
    /// <summary>
    /// The default file name, which is <c>"Trace.log"</c>.
    /// </summary>
    public const string DefaultFileName = "Trace.log";

    /// <summary>
    /// Gets or sets the file name template.
    /// The default value is <c>"Trace.log"</c>.
    /// </summary>
    public string FileNameTemplate { get; set; } = DefaultFileName;

    /// <summary>
    /// Gets or sets the layout of log event.
    /// </summary>
    public string Layout { get; set; } = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${uppercase:${level}:padding=5} ${message}${onexception:inner= }${exception:format=toString}";

    /// <inheritdoc/>
    protected override dynamic GetLogger()
    {
        string uniqueLoggerName = Guid.NewGuid().ToString();
        string filePath = BuildFilePath();

        var target = NLogAdapter.CreateFileTarget(uniqueLoggerName, filePath, Layout);

        NLogAdapter.AddConfigurationRuleForAllLevels(target, uniqueLoggerName);

        return NLogAdapter.GetLogger(uniqueLoggerName);
    }

    /// <inheritdoc/>
    protected override void OnLog(LogEventInfo eventInfo)
    {
        dynamic otherEventInfo = NLogAdapter.CreateLogEventInfo(eventInfo);
        Logger.Log(otherEventInfo);
    }

    private string BuildFilePath()
    {
        AtataContext context = AtataContext.Current;

        string fileName = context.FillPathTemplateString(FileNameTemplate);
        return Path.Combine(context.ArtifactsPath, fileName);
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    object ICloneable.Clone() =>
        new NLogFileConsumer
        {
            FileNameTemplate = FileNameTemplate,
            Layout = Layout
        };
}
