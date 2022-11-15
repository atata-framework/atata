using System;

namespace Atata
{
    /// <summary>
    /// Provides reporting functionality.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class Report<TOwner>
    {
        private readonly TOwner _owner;

        private readonly ILogManager _logManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Report{TOwner}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="logManager">The log manager.</param>
        public Report(TOwner owner, ILogManager logManager)
        {
            _owner = owner;
            _logManager = logManager;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Trace"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Trace(string message, params object[] args)
        {
            _logManager.Trace(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Debug"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Debug(string message, params object[] args)
        {
            _logManager.Debug(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Info"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Info(string message, params object[] args)
        {
            _logManager.Info(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Warn"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Warn(string message, params object[] args)
        {
            _logManager.Warn(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Warn(Exception exception)
        {
            _logManager.Warn(exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Warn(string message, Exception exception = null)
        {
            _logManager.Warn(message, exception);
            return _owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(Exception exception)
        {
            _logManager.Error(exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(string message, Exception exception = null)
        {
            _logManager.Error(message, exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and stack trace at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(string message, string stackTrace)
        {
            _logManager.Error(message, stackTrace);
            return _owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Fatal(Exception exception)
        {
            _logManager.Fatal(exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Fatal(string message, Exception exception = null)
        {
            _logManager.Fatal(message, exception);
            return _owner;
        }

        /// <summary>
        /// Starts the specified log section.
        /// </summary>
        /// <param name="section">The log section.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(LogSection section)
        {
            _logManager.Start(section);
            return _owner;
        }

        /// <summary>
        /// Starts the specified log section with message.
        /// </summary>
        /// <param name="sectionMessage">The section message.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(string sectionMessage)
        {
            _logManager.Start(sectionMessage);
            return _owner;
        }

        /// <summary>
        /// Starts the specified log section with message and log level.
        /// </summary>
        /// <param name="sectionMessage">The section message.</param>
        /// <param name="level">The level.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(string sectionMessage, LogLevel level)
        {
            _logManager.Start(sectionMessage, level);
            return _owner;
        }

        /// <summary>
        /// Ends the latest log section.
        /// </summary>
        /// <returns>The instance of the owner object.</returns>
        public TOwner EndSection()
        {
            _logManager.EndSection();
            return _owner;
        }

        /// <summary>
        /// Takes a screenshot with the optionally specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Screenshot(string title = null)
        {
            _logManager.Screenshot(title);
            return _owner;
        }

        /// <summary>
        /// Takes a snapshot (HTML or MHTML file) of current page with the specified title optionally.
        /// </summary>
        /// <param name="title">The title of a snapshot.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner PageSnapshot(string title = null)
        {
            (_owner as UIComponent)?.Context.TakePageSnapshot(title);
            return _owner;
        }
    }
}
