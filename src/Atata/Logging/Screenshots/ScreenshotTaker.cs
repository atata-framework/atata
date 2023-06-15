using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    internal sealed class ScreenshotTaker
    {
        private readonly IScreenshotStrategy _strategy;

        private readonly AtataContext _context;

        private readonly List<IScreenshotConsumer> _screenshotConsumers = new List<IScreenshotConsumer>();

        private int _screenshotNumber;

        public ScreenshotTaker(
            IScreenshotStrategy strategy,
            AtataContext context)
        {
            _strategy = strategy;
            _context = context;
        }

        // TODO: Atata v3. Delete AddConsumer method.
        public void AddConsumer(IScreenshotConsumer consumer)
        {
            consumer.CheckNotNull(nameof(consumer));

            _screenshotConsumers.Add(consumer);
        }

        public void TakeScreenshot(string title = null)
        {
            if (_strategy is null || !_context.HasDriver || !_screenshotConsumers.Any())
                return;

            _screenshotNumber++;

            try
            {
                string logMessage = $"Take screenshot #{_screenshotNumber:D2}";

                if (!string.IsNullOrWhiteSpace(title))
                    logMessage += $" - {title}";

                _context.Log.Info(logMessage);

                var context = AtataContext.Current;

                FileContentWithExtension fileContent = _strategy.TakeScreenshot(context);

                ScreenshotInfo screenshotInfo = new ScreenshotInfo
                {
                    ScreenshotContent = fileContent,
                    Number = _screenshotNumber,
                    Title = title,
                    PageObjectName = context.PageObject?.ComponentName,
                    PageObjectTypeName = context.PageObject?.ComponentTypeName,
                    PageObjectFullName = context.PageObject?.ComponentFullName
                };

                foreach (IScreenshotConsumer screenshotConsumer in _screenshotConsumers)
                {
                    try
                    {
                        screenshotConsumer.Take(screenshotInfo);
                    }
                    catch (Exception e)
                    {
                        context.Log.Error("Screenshot failed", e);
                    }
                }
            }
            catch (Exception e)
            {
                _context.Log.Error("Screenshot failed", e);
            }
        }
    }
}
