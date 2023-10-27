namespace Atata;

/// <summary>
/// An interface of log writer, an entry point for the Atata logging functionality.
/// </summary>
public interface ILogManager
{
    [Obsolete("Use Trace(string) with string interpolation instead. ")] // Obsolete since v2.12.0.
    void Trace(string message, params object[] args);

    /// <summary>
    /// Writes a trace log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Trace(string message);

    [Obsolete("Use Debug(string) with string interpolation instead. ")] // Obsolete since v2.12.0.
    void Debug(string message, params object[] args);

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Debug(string message);

    [Obsolete("Use Info(string) with string interpolation instead. ")] // Obsolete since v2.12.0.
    void Info(string message, params object[] args);

    /// <summary>
    /// Writes an informational log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Info(string message);

    [Obsolete("Use Warn(string) with string interpolation instead. ")] // Obsolete since v2.12.0.
    void Warn(string message, params object[] args);

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Warn(string message);

    [Obsolete("Use Warn(Exception, string) instead. ")] // Obsolete since v2.12.0.
    void Warn(Exception exception);

    [Obsolete("Use Warn(Exception, string) instead. ")] // Obsolete since v2.12.0.
    void Warn(string message, Exception exception);

    /// <inheritdoc cref="Warn(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    void Warn(Exception exception, string message);

    [Obsolete("Use Error(Exception, string) instead. ")] // Obsolete since v2.12.0.
    void Error(Exception exception);

    [Obsolete("Use Error(Exception, string) instead. ")] // Obsolete since v2.12.0.
    void Error(string message, Exception exception);

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Error(string message);

    /// <inheritdoc cref="Error(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    void Error(Exception exception, string message);

    [Obsolete("Use Fatal(Exception, string) instead. ")] // Obsolete since v2.12.0.
    void Fatal(Exception exception);

    [Obsolete("Use Fatal(Exception, string) instead. ")] // Obsolete since v2.12.0.
    void Fatal(string message, Exception exception);

    /// <summary>
    /// Writes a critical log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Fatal(string message);

    /// <inheritdoc cref="Fatal(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    void Fatal(Exception exception, string message);

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
    [Obsolete("Use TakeScreenshot(...) method of AtataContext or Report.Screenshot(...) instead. For example: AtataContext.Current.TakeScreenshot().")] // Obsolete since v2.4.0.
    void Screenshot(string title = null);
}
