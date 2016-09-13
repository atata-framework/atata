using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace Atata
{
    /// <summary>
    /// Represents the log manager, the entry point for the Atata logging functionality.
    /// </summary>
    /// <seealso cref="Atata.ILogManager" />
    public class LogManager : ILogManager
    {
        private readonly List<LogConsumerInfo> logConsumers = new List<LogConsumerInfo>();
        private readonly List<IScreenshotConsumer> screenshotConsumers = new List<IScreenshotConsumer>();

        private readonly Stack<LogSection> sectionEndStack = new Stack<LogSection>();

        private int screenshotNumber;

        protected IWebDriver Driver
        {
            get { return AtataContext.Current.Driver; }
        }

        /// <summary>
        /// Use the specified consumer for logging.
        /// </summary>
        /// <param name="consumer">The log consumer.</param>
        /// <param name="minLevel">The minimum level of the log message.</param>
        /// <param name="logSectionFinish">If set to <c>true</c> logs the section finish messages with elapsed time span.</param>
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

        public void Error(string message, Exception exception)
        {
            Log(new LogEventInfo
            {
                Level = LogLevel.Error,
                Message = message,
                Exception = exception
            });
        }

        public void Fatal(string message, Exception exception)
        {
            Log(new LogEventInfo
            {
                Level = LogLevel.Fatal,
                Message = message,
                Exception = exception
            });
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

                LogEventInfo eventInfo = new LogEventInfo
                {
                    Level = section.Level,
                    Message = $"Finished: {section.Message} ({Math.Floor(duration.TotalSeconds)}.{duration:fff}s)",
                    SectionEnd = section
                };

                Log(eventInfo, true);
            }
        }

        private void Log(LogLevel level, string message, object[] args)
        {
            Log(new LogEventInfo
            {
                Level = level,
                Message = message.FormatWith(args)
            });
        }

        private void Log(LogEventInfo eventInfo, bool? withLogSectionEnd = null)
        {
            var appropriateConsumers = logConsumers.
                Where(x => eventInfo.Level >= x.MinLevel).
                Where(x => withLogSectionEnd == null || x.LogSectionFinish == withLogSectionEnd).
                Select(x => x.Consumer);

            foreach (ILogConsumer logConsumer in appropriateConsumers)
                logConsumer.Log(eventInfo);
        }

        public void Screenshot(string title = null)
        {
            if (Driver == null || !screenshotConsumers.Any())
                return;

            try
            {
                screenshotNumber++;

                string logMessage = $"Take screenshot #{screenshotNumber}";

                if (!string.IsNullOrWhiteSpace(title))
                    logMessage += $" {title}";

                Info(logMessage);

                ScreenshotInfo screenshotInfo = new ScreenshotInfo
                {
                    Screenshot = Driver.TakeScreenshot(),
                    Number = screenshotNumber,
                    Title = title,
                    PageObjectName = AtataContext.Current.PageObject.ComponentName,
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
