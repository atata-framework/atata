using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class FileScreenshotConsumer : IScreenshotConsumer
    {
        private readonly string folderPath;
        private readonly ImageFormat imageFormat;

        public FileScreenshotConsumer(string folderPath)
            : this(folderPath, ImageFormat.Png)
        {
        }

        public FileScreenshotConsumer(string folderPath, ImageFormat imageFormat)
        {
            this.folderPath = folderPath;
            this.imageFormat = imageFormat;
        }

        public void Take(Screenshot screenshot, int number, string title)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"{number:D2} {SanitizeFileName(title)}.{GetImageFormatExtension(imageFormat)}";
            string filePath = Path.Combine(folderPath, fileName);

            screenshot.SaveAsFile(filePath, imageFormat);
        }

        private string SanitizeFileName(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static string GetImageFormatExtension(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders().First(x => x.FormatID == format.Guid).FilenameExtension;
        }
    }
}
