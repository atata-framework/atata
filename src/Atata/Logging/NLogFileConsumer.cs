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
    /// Gets or sets the builder of the directory path.
    /// </summary>
    public Func<AtataContext, string> DirectoryPathBuilder { get; set; }

    /// <summary>
    /// Gets or sets the builder of the file name.
    /// </summary>
    public Func<AtataContext, string> FileNameBuilder { get; set; }

    /// <summary>
    /// Gets or sets the builder of the file path.
    /// </summary>
    public Func<AtataContext, string> FilePathBuilder { get; set; }

    /// <summary>
    /// Gets or sets the layout of log event.
    /// </summary>
    public string Layout { get; set; } = "${shortdate} ${time} ${uppercase:${level}:padding=5} ${message}${onexception:inner= }${exception:format=toString}";

    /// <inheritdoc/>
    protected override dynamic GetLogger()
    {
        string uniqueLoggerName = Guid.NewGuid().ToString();
        string filePathTemplate = BuildFilePath();

        string filePath = AtataContext.Current.FillTemplateString(filePathTemplate);

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

        if (FilePathBuilder != null)
            return FilePathBuilder.Invoke(context).SanitizeForPath();

        string directoryPath = DirectoryPathBuilder?.Invoke(context)
            ?? BuildDefaultDirectoryPath();

        directoryPath = directoryPath.SanitizeForPath();

        string fileName = FileNameBuilder?.Invoke(context)
            ?? BuildDefaultFileName(context);

        fileName = fileName.SanitizeForFileName();

        return Path.Combine(directoryPath, fileName);
    }

    protected virtual string BuildDefaultDirectoryPath() =>
        AtataContext.Current.Artifacts.FullName;

    protected virtual string BuildDefaultFileName(AtataContext context) =>
        DefaultFileName;

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    object ICloneable.Clone() =>
        new NLogFileConsumer
        {
            DirectoryPathBuilder = DirectoryPathBuilder,
            FileNameBuilder = FileNameBuilder,
            FilePathBuilder = FilePathBuilder,
            Layout = Layout
        };
}
