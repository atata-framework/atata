using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the log manager, the entry point for the Atata logging functionality.
    /// </summary>
    /// <seealso cref="ILogManager" />
    public class LogManager : ILogManager
    {
        private readonly List<LogConsumerInfo> logConsumers = new List<LogConsumerInfo>();
        private readonly List<IScreenshotConsumer> screenshotConsumers = new List<IScreenshotConsumer>();

        private readonly Stack<LogSection> sectionEndStack = new Stack<LogSection>();

        private int screenshotNumber;

        /// <summary>
        /// Use the specified consumer for logging.
        /// </summary>
        /// <param name="consumer">The log consumer.</param>
        /// <param name="minLevel">The minimum level of the log message.</param>
        /// <param name="logSectionFinish">If set to <see langword="true"/> logs the section finish messages with elapsed time span.</param>
        /// <returns>
        /// The same <see cref="LogManager" /> instance.
        /// </returns>
        public LogManager Use(ILogConsumer consumer, LogLevel minLevel = LogLevel.Trace, bool logSectionFinish = true)
        {
            consumer.CheckNotNull(nameof(consumer));

            return Use(new LogConsumerInfo(consumer, minLevel, logSectionFinish));
        }

        internal LogManager Use(LogConsumerInfo consumerInfo)
        {
            logConsumers.Add(consumerInfo);
            return this;
        }

        /// <summary>
        /// Use the specified screenshot consumer.
        /// </summary>
        /// <param name="consumer">The screenshot consumer.</param>
        /// <returns>The same <see cref="LogManager"/> instance.</returns>
        public LogManager Use(IScreenshotConsumer consumer)
        {
            consumer.CheckNotNull(nameof(consumer));

            screenshotConsumers.Add(consumer);
            return this;
        }

        public void Trace(string message, params object[] args)
        {
            Log(LogLevel.Trace, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            Log(LogLevel.Debug, message, args);
        }

        public void Info(string message, params object[] args)
        {
            Log(LogLevel.Info, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Log(LogLevel.Warn, message, args);
        }

        public void Warn(Exception exception)
        {
            Log(LogLevel.Warn, null, exception);
        }

        public void Warn(string message, Exception exception = null)
        {
            Log(LogLevel.Warn, message, exception);
        }

        public void Error(Exception exception)
        {
            Log(LogLevel.Error, null, exception);
        }

        public void Error(string message, Exception exception = null)
        {
            Log(LogLevel.Error, message, exception);
        }

        public void Fatal(Exception exception)
        {
            Log(LogLevel.Fatal, null, exception);
        }

        public void Fatal(string message, Exception exception = null)
        {
            Log(LogLevel.Fatal, message, exception);
        }

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

        /// <summary>
        /// Starts the specified log section.
        /// </summary>
        /// <param name="section">The log section.</param>
        /// <example>This sample shows how to log the data insertion to some control in the scope of the control.
        /// <code>
        /// string value = "new_value";
        /// Log.Start(new DataAdditionLogSection(this, value));
        /// // TODO: Add a value to the control.
        /// Log.EndSection();
        /// </code>
        /// </example>
        public void Start(LogSection section)
        {
            section.CheckNotNull(nameof(section));

            LogEventInfo eventInfo = new LogEventInfo
            {
                Level = section.Level,
                Message = section.Message,
                SectionStart = section
            };

            section.StartedAt = eventInfo.Timestamp;

            Log(eventInfo, false);

            eventInfo.Message = $"Starting: {eventInfo.Message}";

            Log(eventInfo, true);

            sectionEndStack.Push(section);
        }

        /// <summary>
        /// Ends the latest log section.
        /// </summary>
        public void EndSection()
        {
            if (sectionEndStack.Any())
            {
                LogSection section = sectionEndStack.Pop();

                TimeSpan duration = section.GetDuration();

                string message = $"Finished: {section.Message} ({duration.ToLongIntervalString()})";

                if (section.IsResultSet)
                {
                    message = AppendSectionResultToMessage(message, section.Result);
                }
                else if (section.Exception != null)
                {
                    message = AppendSectionResultToMessage(message, section.Exception);
                }

                LogEventInfo eventInfo = new LogEventInfo
                {
                    Level = section.Level,
                    Message = message,
                    SectionEnd = section
                };

                Log(eventInfo, true);
            }
        }

        private static string AppendSectionResultToMessage(string message, object result)
        {
            string resultAsString = result is Exception resultAsException
                ? $"{resultAsException.GetType().FullName}: {resultAsException.Message}"
                : Stringifier.ToString(result);

            string separator = resultAsString.Contains(Environment.NewLine)
                ? Environment.NewLine
                : " ";

            return $"{message} >>{separator}{resultAsString}";
        }

        private void Log(LogLevel level, string message, object[] args)
        {
            string completeMessage = (args?.Length ?? 0) > 0
                ? message.FormatWith(args)
                : message;

            Log(new LogEventInfo
            {
                Level = level,
                Message = completeMessage
            });
        }

        private void Log(LogLevel level, string message, Exception exception)
        {
            Log(new LogEventInfo
            {
                Level = level,
                Message = message,
                Exception = exception
            });
        }

        private void Log(LogEventInfo eventInfo, bool? withLogSectionEnd = null)
        {
            var appropriateConsumerItems = logConsumers.
                Where(x => eventInfo.Level >= x.MinLevel).
                Where(x => withLogSectionEnd == null || x.LogSectionFinish == withLogSectionEnd);

            foreach (var consumerItem in appropriateConsumerItems)
            {
                eventInfo.NestingLevel = sectionEndStack.Count(x => x.Level >= consumerItem.MinLevel);
                consumerItem.Consumer.Log(eventInfo);
            }
        }

        public void Screenshot(string title = null)
        {
            if (AtataContext.Current?.Driver == null || !screenshotConsumers.Any())
                return;

            try
            {
                screenshotNumber++;

                string logMessage = $"Take screenshot #{screenshotNumber:D2}";

                if (!string.IsNullOrWhiteSpace(title))
                    logMessage += $" - {title}";

                Info(logMessage);

                ScreenshotInfo screenshotInfo = new ScreenshotInfo
                {
                    Screenshot = AtataContext.Current.Driver.GetScreenshot(),
                    Number = screenshotNumber,
                    Title = title,
                    PageObjectName = AtataContext.Current.PageObject.ComponentName,
                    PageObjectTypeName = AtataContext.Current.PageObject.ComponentTypeName,
                    PageObjectFullName = AtataContext.Current.PageObject.ComponentFullName
                };

                foreach (IScreenshotConsumer screenshotConsumer in screenshotConsumers)
                {
                    try
                    {
                        screenshotConsumer.Take(screenshotInfo);
                    }
                    catch (Exception e)
                    {
                        Error("Screenshot failed", e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("Screenshot failed", e);
            }
        }
    }
}
