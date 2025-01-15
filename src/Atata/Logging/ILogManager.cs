namespace Atata;

/// <summary>
/// An interface of log writer, an entry point for the Atata logging functionality.
/// </summary>
public interface ILogManager
{
    /// <summary>
    /// Logs an entry.
    /// </summary>
    /// <param name="level">The level.</param>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    void Log(LogLevel level, string message, Exception exception = null);

    /// <summary>
    /// Logs an entry.
    /// </summary>
    /// <param name="utcTimestamp">The timestamp in UTC form.</param>
    /// <param name="level">The level.</param>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    void Log(DateTime utcTimestamp, LogLevel level, string message, Exception exception = null);

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

    /// <summary>
    /// Gets or creates another log manager with the same configuration that additionally sets
    /// <see cref="LogEventInfo.ExternalSource"/> of a log event with
    /// the specified <paramref name="externalSource"/>.
    /// </summary>
    /// <param name="externalSource">The external source name.</param>
    /// <returns>An <see cref="ILogManager"/> instance for the external source.</returns>
    ILogManager ForExternalSource(string externalSource);

    /// <summary>
    /// Gets or creates another log manager with the same configuration that additionally sets
    /// <see cref="LogEventInfo.Category"/> of a log event with
    /// the specified <paramref name="category"/>.
    /// </summary>
    /// <param name="category">The category name.</param>
    /// <returns>An <see cref="ILogManager"/> instance for the category.</returns>
    ILogManager ForCategory(string category);

    /// <summary>
    /// Gets or creates another log manager with the same configuration that additionally sets
    /// <see cref="LogEventInfo.Category"/> of a log event with
    /// the specified <typeparamref name="TCategory"/> type name.
    /// </summary>
    /// <typeparam name="TCategory">The type of the category.</typeparam>
    /// <returns>An <see cref="ILogManager"/> instance for the category.</returns>
    ILogManager ForCategory<TCategory>();

    /// <summary>
    /// Creates a sub-logger with the same configuration.
    /// The sub-logger starts within parent logger's log sections hierarchy,
    /// but follows the hierarchy only on start position
    /// and doesn't reflect further section hierarchy changes of parent logger.
    /// Sub-loggers are intended for logging of actions executed in additional threads.
    /// </summary>
    /// <returns>A new sub-logger <see cref="ILogManager"/> instance.</returns>
    ILogManager CreateSubLog();

    /// <summary>
    /// Creates a sub-logger with the same configuration that additionally sets
    /// <see cref="LogEventInfo.Category"/> of a log event with
    /// the specified <paramref name="category"/>.
    /// The sub-logger starts within parent logger's log sections hierarchy,
    /// but follows the hierarchy only on start position
    /// and doesn't reflect further section hierarchy changes of parent logger.
    /// Sub-loggers are intended for logging of actions executed in additional threads.
    /// </summary>
    /// <param name="category">The category name.</param>
    /// <returns>A new sub-logger <see cref="ILogManager"/> instance for the category.</returns>
    ILogManager CreateSubLogForCategory(string category);
}
