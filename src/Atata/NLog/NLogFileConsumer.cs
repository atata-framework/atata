﻿#nullable enable

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
    /// The default value is <c>@"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-external-source}'!='':inner={${event-property:log-external-source}\} }${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}"</c>.
    /// </summary>
    /// <remarks>
    /// If you want to replace "time elapsed" column in layout with "timestamp", you can replace the value
    /// <c>"{event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff}"</c> with
    /// <c>"{date:format=yyyy-MM-dd HH\:mm\:ss.fff}"</c>.
    /// </remarks>
    public string Layout { get; set; } =
        @"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-external-source}'!='':inner={${event-property:log-external-source}\} }${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}";

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
        Logger!.Log(otherEventInfo);
    }

    private string BuildFilePath()
    {
        AtataContext context = AtataContext.ResolveCurrent();

        string fileName = context.Variables.FillPathTemplateString(FileNameTemplate);
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
