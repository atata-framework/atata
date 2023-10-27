namespace Atata;

/// <summary>
/// An interface of log writer, an entry point for the Atata logging functionality.
/// </summary>
public interface ILogManager
{
    /// <summary>
    /// Writes a trace log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="args">The message arguments.</param>
    void Trace(string message, params object[] args);

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="args">The message arguments.</param>
    void Debug(string message, params object[] args);

    /// <summary>
    /// Writes an informational log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="args">The message arguments.</param>
    void Info(string message, params object[] args);

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="args">The message arguments.</param>
    void Warn(string message, params object[] args);

    /// <summary>
    /// Writes the exception as a warning log message.
    /// </summary>
    /// <param name="exception">The exception.</param>
    void Warn(Exception exception);

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    void Warn(string message, Exception exception = null);

    /// <summary>
    /// Writes the exception as an error log message.
    /// </summary>
    /// <param name="exception">The exception.</param>
    void Error(Exception exception);

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    void Error(string message, Exception exception = null);

    /// <summary>
    /// Writes the exception as a critical log message.
    /// </summary>
    /// <param name="exception">The exception.</param>
    void Fatal(Exception exception);

    /// <summary>
    /// Writes a critical log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    void Fatal(string message, Exception exception = null);

    /// <summary>
    /// Executes the action within the log section.
    /// Writes start and end log messages.
    /// Writes exception to the end log message, if it occurs.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="action">The action to execute.</param>
    void ExecuteSection(LogSection section, Action action);

    /// <summary>
    /// Executes the function within the log section.
    /// Writes start and end log messages.
    /// Writes exception to the end log message, if it occurs.
    /// Also writes result of the <paramref name="function"/> to the end log message.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="section">The section.</param>
    /// <param name="function">The function to execute.</param>
    /// <returns>The result of <paramref name="function"/>.</returns>
    TResult ExecuteSection<TResult>(LogSection section, Func<TResult> function);

    /// <summary>
    /// Starts the specified log section.
    /// </summary>
    /// <param name="section">The log section.</param>
    void Start(LogSection section);

    /// <summary>
    /// Ends the latest log section.
    /// </summary>
    void EndSection();

    /// <summary>
    /// Takes a screenshot of current page with the specified title optionally.
    /// </summary>
    /// <param name="title">The title of screenshot.</param>
    [Obsolete("Use TakeScreenshot(...) method of AtataContext instead. For example: AtataContext.Current.TakeScreenshot().")] // Obsolete since v2.4.0.
    void Screenshot(string title = null);
}
