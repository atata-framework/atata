using OpenQA.Selenium;

namespace Atata
{
    public static class ScreenshotImageFormatExtensions
    {
        public static string GetExtension(this ScreenshotImageFormat format)
        {
            if (format == ScreenshotImageFormat.Jpeg)
                return ".jpg";
            else
                return format.ToString().ToLower().Prepend(".");
        }
    }
}
