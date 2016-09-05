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
        private readonly List<ILogConsumer> logConsumers = new List<ILogConsumer>();
        private readonly List<IScreenshotConsumer> screenshotConsumers = new List<IScreenshotConsumer>();

        private readonly Stack<LogSection> sectionEndStack = new Stack<LogSection>();

        private int screenshotNumber;

        protected IWebDriver Driver
        {
            get { return ATContext.Current.Driver; }
        }

        /// <summary>
        /// Use the specified consumer for logging.
        /// </summary>
        /// <param name="consumer">The log consumer.</param>
        /// <returns></returns>
        public LogManager Use(ILogConsumer consumer)
        {
            logConsumers.Add(consumer);
            return this;
        }

        /// <summary>
        /// Use the specified screenshot consumer.
        /// </summary>
        /// <param name="consumer">The screenshot consumer.</param>
        /// <returns></returns>
        public LogManager Use(IScreenshotConsumer consumer)
        {
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
        /// <example> This sample shows how to log the data insertion to some control in the scope of the control.
        /// <code>
        /// string value = "new_value";
        /// Log.Start(new DataAdditionLogSection(this, value));
        /// // TODO: Add a value to the control.
        /// Log.EndSection();
        /// </code>
        /// </example>
        public void Start(LogSection section)
        {
            LogEventInfo eventInfo = new LogEventInfo
            {
                Level = section.Level,
                Message = $"Starting: {section.Message}",
                SectionStart = section
            };

            section.StartedAt = eventInfo.Timestamp;

            Log(eventInfo);

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

                Log(eventInfo);
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

        private void Log(LogEventInfo eventInfo)
        {
            foreach (ILogConsumer logConsumer in logConsumers)
                logConsumer.Log(eventInfo);
        }

        public void Screenshot(string title = null)
        {
            if (Driver == null || !screenshotConsumers.Any())
                return;

            try
            {
                screenshotNumber++;

                Info($"Take screenshot {screenshotNumber} {title}");

                Screenshot screenshot = Driver.TakeScreenshot();

                foreach (IScreenshotConsumer screenshotConsumer in screenshotConsumers)
                {
                    try
                    {
                        screenshotConsumer.Take(screenshot, screenshotNumber, title);
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
