#nullable enable

namespace Atata;

/// <summary>
/// An interface of reporting functionality belonging to particular <typeparamref name="TOwner"/>.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public interface IReport<out TOwner>
{
    /// <summary>
    /// Writes a trace log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Trace(string message);

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Debug(string message);

    /// <summary>
    /// Writes an informational log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Info(string message);

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Warn(string message);

    /// <inheritdoc cref="Warn(string)"/>
    /// <param name="exception">The exception.</param>
    TOwner Warn(Exception exception);

    /// <inheritdoc cref="Warn(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    TOwner Warn(Exception exception, string message);

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Error(string message);

    /// <inheritdoc cref="Error(string)"/>
    /// <param name="exception">The exception.</param>
    TOwner Error(Exception exception);

    /// <inheritdoc cref="Error(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    TOwner Error(Exception exception, string message);

    /// <summary>
    /// Writes a critical log message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Fatal(string message);

    /// <inheritdoc cref="Fatal(string)"/>
    /// <param name="exception">The exception.</param>
    TOwner Fatal(Exception exception);

    /// <inheritdoc cref="Fatal(string)"/>
    /// <param name="exception">The exception.</param>
    /// <param name="message">The message.</param>
    TOwner Fatal(Exception exception, string message);

    /// <summary>
    /// Executes the specified action and represents it in a log as a setup section with the specified message.
    /// The setup action time is not counted as a "Test body" execution time, but counted as "Setup" time.
    /// </summary>
    /// <param name="message">The setup message.</param>
    /// <param name="action">The setup action.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Setup(string message, Action<TOwner> action);

    /// <summary>
    /// Executes the specified function and represents it in a log as a setup section with the specified message.
    /// The setup function time is not counted as a "Test body" execution time, but counted as "Setup" time.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="message">The setup message.</param>
    /// <param name="function">The setup function.</param>
    /// <returns>The result of the <paramref name="function"/>.</returns>
    TResult Setup<TResult>(string message, Func<TOwner, TResult> function);

    /// <summary>
    /// Executes asynchronously the specified task-based function and represents it in a log as a setup section with the specified message.
    /// The setup action time is not counted as a "Test body" execution time, but counted as "Setup" time.
    /// </summary>
    /// <param name="message">The setup message.</param>
    /// <param name="function">The setup function.</param>
    /// <returns>The <see cref="Task"/> object.</returns>
    Task SetupAsync(string message, Func<TOwner, Task> function);

    /// <summary>
    /// Executes asynchronously the specified task-based function and represents it in a log as a setup section with the specified message.
    /// The setup function time is not counted as a "Test body" execution time, but counted as "Setup" time.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="message">The setup message.</param>
    /// <param name="function">The setup function.</param>
    /// <returns>The <see cref="Task{TResult}"/> object with the result of the <paramref name="function"/>.</returns>
    Task<TResult> SetupAsync<TResult>(string message, Func<TOwner, Task<TResult>> function);

    /// <summary>
    /// Executes the specified action and represents it in a log as a section with the specified message.
    /// </summary>
    /// <param name="message">The step message.</param>
    /// <param name="action">The step action.</param>
    /// <returns>The instance of the owner object.</returns>
    TOwner Step(string message, Action<TOwner> action);

    /// <summary>
    /// Executes the specified function and represents it in a log as a section with the specified message.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="message">The step message.</param>
    /// <param name="function">The step function.</param>
    /// <returns>The result of the <paramref name="function"/>.</returns>
    TResult Step<TResult>(string message, Func<TOwner, TResult> function);

    /// <summary>
    /// Executes asynchronously the specified task-based function and represents it in a log as a section with the specified message.
    /// </summary>
    /// <param name="message">The step message.</param>
    /// <param name="function">The step action.</param>
    /// <returns>The <see cref="Task"/> object.</returns>
    Task StepAsync(string message, Func<TOwner, Task> function);

    /// <summary>
    /// Executes asynchronously the specified task-based function and represents it in a log as a section with the specified message.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="message">The step message.</param>
    /// <param name="function">The step function.</param>
    /// <returns>The <see cref="Task{TResult}"/> object with the result of the <paramref name="function"/>.</returns>
    Task<TResult> StepAsync<TResult>(string message, Func<TOwner, Task<TResult>> function);
}
