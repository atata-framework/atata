using System;

namespace Atata
{
    /// <summary>
    /// Provides reporting functionality.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class Report<TOwner>
    {
        private readonly TOwner owner;

        private readonly ILogManager logManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Report{TOwner}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="logManager">The log manager.</param>
        public Report(TOwner owner, ILogManager logManager)
        {
            this.owner = owner;
            this.logManager = logManager;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Trace"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Trace(string message, params object[] args)
        {
            logManager.Trace(message, args);
            return owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Debug"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Debug(string message, params object[] args)
        {
            logManager.Debug(message, args);
            return owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Info"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Info(string message, params object[] args)
        {
            logManager.Info(message, args);
            return owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Warn"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Warn(string message, params object[] args)
        {
            logManager.Warn(message, args);
            return owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(Exception exception)
        {
            logManager.Error(exception);
            return owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(string message, Exception exception = null)
        {
            logManager.Error(message, exception);
            return owner;
        }

        /// <summary>
        /// Writes the log message and stack trace at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(string message, string stackTrace)
        {
            logManager.Error(message, stackTrace);
            return owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Fatal(Exception exception)
        {
            logManager.Fatal(exception);
            return owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Fatal(string message, Exception exception = null)
        {
            logManager.Fatal(message, exception);
            return owner;
        }

        /// <summary>
        /// Starts the specified log section.
        /// </summary>
        /// <param name="section">The log section.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(LogSection section)
        {
            logManager.Start(section);
            return owner;
        }

        /// <summary>
        /// Starts the specified log section with message.
        /// </summary>
        /// <param name="sectionMessage">The section message.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(string sectionMessage)
        {
            logManager.Start(sectionMessage);
            return owner;
        }

        /// <summary>
        /// Starts the specified log section with message and log level.
        /// </summary>
        /// <param name="sectionMessage">The section message.</param>
        /// <param name="level">The level.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(string sectionMessage, LogLevel level)
        {
            logManager.Start(sectionMessage, level);
            return owner;
        }

        /// <summary>
        /// Ends the latest log section.
        /// </summary>
        /// <returns>The instance of the owner object.</returns>
        public TOwner EndSection()
        {
            logManager.EndSection();
            return owner;
        }

        /// <summary>
        /// Takes a screenshot with the optionally specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Screenshot(string title = null)
        {
            logManager.Screenshot(title);
            return owner;
        }
    }
}
