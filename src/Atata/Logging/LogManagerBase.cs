using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Atata
{
    public abstract class LogManagerBase : ILogManager
    {
        private int screenshotNumber = 0;

        public LogManagerBase(IWebDriver driver = null, string screenshotsFolderPath = null)
        {
            Driver = driver;
            ScreenshotsFolderPath = screenshotsFolderPath;
        }

        public IWebDriver Driver { get; set; }
        public string ScreenshotsFolderPath { get; set; }

        public abstract void Info(string message, params object[] args);
        public abstract void Warn(string message, params object[] args);
        public abstract void Error(string message, System.Exception excepton);

        public void Screenshot(string title = null)
        {
            if (ScreenshotsFolderPath == null || Driver == null)
                return;

            try
            {
                if (screenshotNumber == 0 && !Directory.Exists(ScreenshotsFolderPath))
                    Directory.CreateDirectory(ScreenshotsFolderPath);
                screenshotNumber++;

                string fullTitle = GetScreenshotFullTitle(title);
                Info("Take screenshot {0}", fullTitle);

                string fileName = string.Format("{0}.png", fullTitle);
                string filePath = Path.Combine(ScreenshotsFolderPath, fileName);
                Screenshot screenshot = Driver.TakeScreenshot();
                screenshot.SaveAsFile(filePath, ImageFormat.Png);
            }
            catch (Exception e)
            {
                Error("Screenshot failed", e);
            }
        }

        protected virtual string GetScreenshotFullTitle(string title)
        {
            StringBuilder builder = new StringBuilder(screenshotNumber.ToString("D2"));
            if (!string.IsNullOrWhiteSpace(title))
                builder.AppendFormat(" - {0}", title);

            return builder.ToString();
        }
    }
}
