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
    void Trace(string message);

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Debug(string message);

    /// <summary>
    /// Writes an informational log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Info(string message);

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Warn(string message);

    /// <inheritdoc cref="Warn(string)"/>
    /// <param name="exception">The exception.</param>
    void Warn(Exception exception);

    /// <inheritdoc cref="Warn(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    void Warn(Exception exception, string message);

    /// <inheritdoc cref="Error(string)"/>
    /// <param name="exception">The exception.</param>
    void Error(Exception exception);

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Error(string message);

    /// <inheritdoc cref="Error(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    void Error(Exception exception, string message);

    /// <inheritdoc cref="Fatal(string)"/>
    /// <param name="exception">The exception.</param>
    void Fatal(Exception exception);

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
    /// Executes asynchronously the task-based function within the log section.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="function">The function to execute.</param>
    /// <returns>The <see cref="Task"/> object.</returns>
    Task ExecuteSectionAsync(LogSection section, Func<Task> function);

    /// <summary>
    /// Executes asynchronously the task-based function within the log section.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="section">The section.</param>
    /// <param name="function">The function to execute.</param>
    /// <returns>The <see cref="Task{TResult}"/> object with the result of the <paramref name="function"/>.</returns>
    Task<TResult> ExecuteSectionAsync<TResult>(LogSection section, Func<Task<TResult>> function);
}
