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
    /// The default value is <c>"yyyy-MM-dd HH:mm:ss.ffff"</c>.
    /// </summary>
    public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.ffff";

    public void Log(LogEventInfo eventInfo)
    {
        string completeMessage = BuildCompleteMessage(eventInfo);
        Write(completeMessage);
    }

    protected virtual void Write(string completeMessage) =>
        _writeAction?.Invoke(completeMessage);

    private string BuildCompleteMessage(LogEventInfo eventInfo)
    {
        StringBuilder builder = new StringBuilder()
            .Append(eventInfo.Timestamp.ToString(TimestampFormat, CultureInfo.InvariantCulture))
            .Append(Separator)
            .Append($"{eventInfo.Level.ToString(TermCase.Upper),5}")
            .Append(Separator)
            .Append(eventInfo.Message);

        if (eventInfo.Exception != null)
            builder.AppendIf(!string.IsNullOrWhiteSpace(eventInfo.Message), Separator)
                .Append(eventInfo.Exception.ToString());

        return builder.ToString();
    }
}
