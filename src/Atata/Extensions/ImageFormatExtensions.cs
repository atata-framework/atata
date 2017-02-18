using System;
using System.Drawing.Imaging;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public static class ImageFormatExtensions
    {
        public static string GetExtension(this ImageFormat format)
        {
            format.CheckNotNull(nameof(format));

            return ImageCodecInfo.GetImageEncoders().
                First(x => x.FormatID == format.Guid).
                FilenameExtension.
                Split(';').
                First().
                TrimStart('*').
                ToLower();
        }

        public static ScreenshotImageFormat ToScreenshotImageFormat(this ImageFormat format)
        {
            format.CheckNotNull(nameof(format));

            if (format == ImageFormat.Png)
                return ScreenshotImageFormat.Png;
            else if (format == ImageFormat.Jpeg)
                return ScreenshotImageFormat.Jpeg;
            else if (format == ImageFormat.Gif)
                return ScreenshotImageFormat.Gif;
            else if (format == ImageFormat.Tiff)
                return ScreenshotImageFormat.Tiff;
            else if (format == ImageFormat.Bmp)
                return ScreenshotImageFormat.Bmp;
            else
                throw new ArgumentException($"Unsupported value: {format}", nameof(format));
        }
    }
}
