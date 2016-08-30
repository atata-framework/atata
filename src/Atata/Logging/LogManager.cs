using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace Atata
{
    public class LogManager : ILogManager
    {
        private readonly List<ILogConsumer> logConsumers = new List<ILogConsumer>();

        [ThreadStatic]
        private static Stack<LogSection> sectionEndStack;

        private int screenshotNumber;

        public LogManager(string screenshotsFolderPath = null)
        {
            ScreenshotsFolderPath = screenshotsFolderPath;
        }

        protected IWebDriver Driver
        {
            get { return ATContext.Current.Driver; }
        }

        private static Stack<LogSection> SectionEndStack
        {
            get { return sectionEndStack ?? (sectionEndStack = new Stack<LogSection>()); }
        }

        public string ScreenshotsFolderPath { get; set; }

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

            SectionEndStack.Push(section);
        }

        public void EndSection()
        {
            if (SectionEndStack.Any())
            {
                LogSection section = SectionEndStack.Pop();

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
            if (ScreenshotsFolderPath == null || Driver == null)
                return;

            try
            {
                if (screenshotNumber == 0 && !Directory.Exists(ScreenshotsFolderPath))
                    Directory.CreateDirectory(ScreenshotsFolderPath);
                screenshotNumber++;

                string completeTitle = GetScreenshotCompleteTitle(title);
                Info("Take screenshot {0}", completeTitle);

                string fileName = string.Format("{0}.png", completeTitle);
                string filePath = Path.Combine(ScreenshotsFolderPath, fileName);
                Screenshot screenshot = Driver.TakeScreenshot();
                screenshot.SaveAsFile(filePath, ImageFormat.Png);
            }
            catch (Exception e)
            {
                Error("Screenshot failed", e);
            }
        }

        protected virtual string GetScreenshotCompleteTitle(string title)
        {
            StringBuilder builder = new StringBuilder(screenshotNumber.ToString("D2"));
            if (!string.IsNullOrWhiteSpace(title))
                builder.AppendFormat(" - {0}", title);

            return builder.ToString();
        }
    }
}
