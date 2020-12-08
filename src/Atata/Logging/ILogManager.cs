using System;

namespace Atata
{
    public interface ILogManager
    {
        void Trace(string message, params object[] args);

        void Debug(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warn(string message, params object[] args);

        void Warn(Exception exception);

        void Warn(string message, Exception exception = null);

        void Error(Exception exception);

        void Error(string message, Exception exception = null);

        void Fatal(Exception exception);

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

        void Start(LogSection section);

        void EndSection();

        void Screenshot(string title = null);
    }
}
