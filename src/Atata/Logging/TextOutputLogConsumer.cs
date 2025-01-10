namespace Atata;

/// <summary>
/// Represents text output log consumer.
/// Is used for regular text logging.
/// Action to write text to some source can be passed via constructor.
/// Also it is possible to define custom class inherited from <see cref="TextOutputLogConsumer"/> and override <see cref="Write(string)"/> method.
/// </summary>
public class TextOutputLogConsumer : ILogConsumer
{
    private readonly Action<string> _writeAction;

    public TextOutputLogConsumer()
    {
    }

    public TextOutputLogConsumer(Action<string> writeAction) =>
        _writeAction = writeAction;

    /// <summary>
    /// Gets or sets the text parts separator.
    /// The default value is <c>" "</c>.
    /// </summary>
    public string Separator { get; set; } = " ";

    /// <summary>
    /// Gets or sets the timestamp format.
    /// The default value is <c>"yyyy-MM-dd HH:mm:ss.fff"</c>.
    /// </summary>
    public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

    /// <summary>
    /// Gets or sets the time elapsed format.
    /// The default value is <c>@"hh\:mm\:ss\.fff"</c>.
    /// </summary>
    public string TimeElapsedFormat { get; set; } = @"hh\:mm\:ss\.fff";

    /// <summary>
    /// Gets or sets a value indicating whether to output <see cref="LogEventInfo.Timestamp"/>
    /// instead of <see cref="LogEventInfo.TimeElapsed"/>.
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool OutputTimestamp { get; set; }

    public void Log(LogEventInfo eventInfo)
    {
        string completeMessage = BuildCompleteMessage(eventInfo);
        Write(completeMessage);
    }

    protected virtual void Write(string completeMessage) =>
        _writeAction?.Invoke(completeMessage);

    private string BuildCompleteMessage(LogEventInfo eventInfo)
    {
        var builder = new StringBuilder()
            .Append(
                OutputTimestamp
                    ? eventInfo.Timestamp.ToString(TimestampFormat, CultureInfo.InvariantCulture)
                    : eventInfo.TimeElapsed.ToString(TimeElapsedFormat, CultureInfo.InvariantCulture))
            .Append(Separator)
            .Append(eventInfo.ExecutionUnitId)
            .Append(Separator)
            .Append($"{eventInfo.Level.ToString(TermCase.Upper),5}")
            .Append(Separator)
            .Append(eventInfo.NestingText);

        bool hasExternalSource = !string.IsNullOrEmpty(eventInfo.ExternalSource);

        if (hasExternalSource)
            builder.AppendFormat("{{{0}}}", eventInfo.ExternalSource);

        bool hasCategory = !string.IsNullOrEmpty(eventInfo.Category);

        if (hasCategory)
        {
            if (hasExternalSource)
                builder.Append(Separator);

            builder.AppendFormat("[{0}]", eventInfo.Category);
        }

        bool hasMessage = !string.IsNullOrEmpty(eventInfo.Message);

        if (hasMessage)
        {
            if (hasCategory || hasExternalSource)
                builder.Append(Separator);

            builder.Append(eventInfo.Message);
        }

        if (eventInfo.Exception != null)
        {
            if (hasMessage || hasCategory || hasExternalSource)
                builder.Append(Separator);

            builder.Append(eventInfo.Exception.ToString());
        }

        return builder.ToString();
    }
}
