using OpenQA.Selenium;

namespace Atata
{
    public static class ScreenshotImageFormatExtensions
    {
        public static string GetExtension(this ScreenshotImageFormat format)
        {
            return format == ScreenshotImageFormat.Jpeg
                ? ".jpg"
                : format.ToString().ToLower().Prepend(".");
        }
    }
}
