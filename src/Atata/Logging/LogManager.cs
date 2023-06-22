using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents the log manager, an entry point for the Atata logging functionality.
    /// </summary>
    /// <seealso cref="ILogManager" />
    public class LogManager : ILogManager
    {
        private readonly ILogEventInfoFactory _logEventInfoFactory;

        private readonly List<LogConsumerConfiguration> _logConsumerConfigurations = new();

        private readonly List<SecretStringToMask> _secretStringsToMask = new();

        private readonly Stack<LogSection> _sectionEndStack = new();

        public LogManager(ILogEventInfoFactory logEventInfoFactory) =>
            _logEventInfoFactory = logEventInfoFactory.CheckNotNull(nameof(logEventInfoFactory));

        /// <summary>
        /// Use the specified consumer configuration for logging.
        /// </summary>
        /// <param name="logConsumerConfiguration">The log consumer configuration.</param>
        /// <returns>
        /// The same <see cref="LogManager" /> instance.
        /// </returns>
        public LogManager Use(LogConsumerConfiguration logConsumerConfiguration)
        {
            logConsumerConfiguration.CheckNotNull(nameof(logConsumerConfiguration));

            _logConsumerConfigurations.Add(logConsumerConfiguration);

            return this;
        }

        /// <summary>
        /// Use the specified screenshot consumer.
        /// </summary>
        /// <param name="consumer">The screenshot consumer.</param>
        /// <returns>The same <see cref="LogManager"/> instance.</returns>
        [Obsolete("Don't use this method. Configure screenshot consumers through the AtataContetBuilder instead.")] // Obsolete since v2.8.0.
        public LogManager Use(IScreenshotConsumer consumer)
        {
            AtataContext.Current?.ScreenshotTaker.AddConsumer(consumer);

            return this;
        }

        /// <summary>
        /// Adds the secret strings to mask.
        /// </summary>
        /// <param name="secretStringsToMask">The secret strings to mask.</param>
        /// <returns>The same <see cref="LogManager"/> instance.</returns>
        public LogManager AddSecretStringsToMask(IEnumerable<SecretStringToMask> secretStringsToMask)
        {
            secretStringsToMask.CheckNotNull(nameof(secretStringsToMask));

            _secretStringsToMask.AddRange(secretStringsToMask);

            return this;
        }

        /// <inheritdoc/>
        public void Trace(string message, params object[] args) =>
            Log(LogLevel.Trace, message, args);

        /// <inheritdoc/>
        public void Debug(string message, params object[] args) =>
            Log(LogLevel.Debug, message, args);

        /// <inheritdoc/>
        public void Info(string message, params object[] args) =>
            Log(LogLevel.Info, message, args);

        /// <inheritdoc/>
        public void Warn(string message, params object[] args) =>
            Log(LogLevel.Warn, message, args);

        /// <inheritdoc/>
        public void Warn(Exception exception) =>
            Log(LogLevel.Warn, null, exception);

        /// <inheritdoc/>
        public void Warn(string message, Exception exception = null) =>
            Log(LogLevel.Warn, message, exception);

        /// <inheritdoc/>
        public void Error(Exception exception) =>
            Log(LogLevel.Error, null, exception);

        /// <inheritdoc/>
        public void Error(string message, Exception exception = null) =>
            Log(LogLevel.Error, message, exception);

        /// <inheritdoc/>
        public void Fatal(Exception exception) =>
            Log(LogLevel.Fatal, null, exception);

        /// <inheritdoc/>
        public void Fatal(string message, Exception exception = null) =>
            Log(LogLevel.Fatal, message, exception);

        /// <inheritdoc/>
        public void ExecuteSection(LogSection section, Action action)
        {
            action.CheckNotNull(nameof(action));

            Start(section);

            try
            {
                action?.Invoke();
            }
            catch (Exception exception)
            {
                section.Exception = exception;
                throw;
            }
            finally
            {
                EndSection();
            }
        }

        /// <inheritdoc/>
        public TResult ExecuteSection<TResult>(LogSection section, Func<TResult> function)
        {
            function.CheckNotNull(nameof(function));

            Start(section);

            try
            {
                TResult result = function.Invoke();
                section.Result = result;
                return result;
            }
            catch (Exception exception)
            {
                section.Exception = exception;
                throw;
            }
            finally
            {
                EndSection();
            }
        }

        /// <inheritdoc/>
        public void Start(LogSection section)
        {
            section.CheckNotNull(nameof(section));

            LogEventInfo eventInfo = _logEventInfoFactory.Create(section.Level, section.Message);
            eventInfo.SectionStart = section;

            section.StartedAt = eventInfo.Timestamp;
            section.Stopwatch.Start();

            Log(eventInfo);

            _sectionEndStack.Push(section);
        }

        /// <inheritdoc/>
        public void EndSection()
        {
            if (_sectionEndStack.Any())
            {
                LogSection section = _sectionEndStack.Pop();

                section.Stopwatch.Stop();

                string message = $"{section.Message} ({section.ElapsedTime.ToLongIntervalString()})";

                if (section.IsResultSet)
                    message = AppendSectionResultToMessage(message, section.Result);
                else if (section.Exception != null)
                    message = AppendSectionResultToMessage(message, section.Exception);

                LogEventInfo eventInfo = _logEventInfoFactory.Create(section.Level, message);
                eventInfo.SectionEnd = section;

                Log(eventInfo);
            }
        }

        private static string AppendSectionResultToMessage(string message, object result)
        {
            string resultAsString = result is Exception resultAsException
                ? BuildExceptionShortSingleLineMessage(resultAsException)
                : Stringifier.ToString(result);

            string separator = resultAsString.Contains(Environment.NewLine)
                ? Environment.NewLine
                : " ";

            return $"{message} >>{separator}{resultAsString}";
        }

        private static string BuildExceptionShortSingleLineMessage(Exception exception)
        {
            string message = exception.Message;

            int newLineIndex = message.IndexOf(Environment.NewLine, StringComparison.Ordinal);

            if (newLineIndex >= 0)
            {
                message = message.Substring(0, newLineIndex);

                message += message[message.Length - 1] == '.'
                    ? ".."
                    : "...";
            }

            return $"{exception.GetType().FullName}: {message}";
        }

        private static string PrependHierarchyPrefixesToMessage(string message, LogEventInfo eventInfo, LogConsumerConfiguration logConsumerConfiguration)
        {
            StringBuilder builder = new StringBuilder();

            if (eventInfo.NestingLevel > 0)
            {
                for (int i = 0; i < eventInfo.NestingLevel; i++)
                {
                    builder.Append(logConsumerConfiguration.MessageNestingLevelIndent);
                }
            }

            if (logConsumerConfiguration.LogSectionFinish)
            {
                if (eventInfo.SectionStart != null)
                    builder.Append(logConsumerConfiguration.MessageStartSectionPrefix);
                else if (eventInfo.SectionEnd != null)
                    builder.Append(logConsumerConfiguration.MessageEndSectionPrefix);
            }

            string resultMessage = builder.Append(message).ToString();

            return resultMessage.Length == 0 && message == null
                ? null
                : resultMessage;
        }

        private void Log(LogLevel level, string message, object[] args)
        {
            string completeMessage = (args?.Length ?? 0) > 0
                ? message.FormatWith(args)
                : message;

            LogEventInfo logEvent = _logEventInfoFactory.Create(level, completeMessage);

            Log(logEvent);
        }

        private void Log(LogLevel level, string message, Exception exception)
        {
            LogEventInfo logEvent = _logEventInfoFactory.Create(level, message);
            logEvent.Exception = exception;

            Log(logEvent);
        }

        private void Log(LogEventInfo eventInfo)
        {
            var appropriateConsumerItems = _logConsumerConfigurations
                .Where(x => eventInfo.Level >= x.MinLevel);

            if (eventInfo.SectionEnd != null)
            {
                appropriateConsumerItems = appropriateConsumerItems
                    .Where(x => x.LogSectionFinish);
            }

            string originalMessage = ApplySecretMasks(eventInfo.Message);

            foreach (var consumerItem in appropriateConsumerItems)
            {
                eventInfo.NestingLevel = _sectionEndStack.Count(x => x.Level >= consumerItem.MinLevel);
                eventInfo.Message = PrependHierarchyPrefixesToMessage(originalMessage, eventInfo, consumerItem);

                consumerItem.Consumer.Log(eventInfo);
            }
        }

        private string ApplySecretMasks(string message)
        {
            foreach (var secret in _secretStringsToMask)
                message = message.Replace(secret.Value, secret.Mask);

            return message;
        }

        [Obsolete("Use TakeScreenshot(...) method of AtataContext instead. For example: AtataContext.Current.TakeScreenshot().")] // Obsolete since v2.8.0.
        public void Screenshot(string title = null) =>
            AtataContext.Current?.TakeScreenshot(title);
    }
}
