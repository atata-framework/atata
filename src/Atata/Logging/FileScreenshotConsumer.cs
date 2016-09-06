using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the screenshot consumer that saves the screenshot to the file.
    /// </summary>
    /// <seealso cref="Atata.IScreenshotConsumer" />
    public class FileScreenshotConsumer : IScreenshotConsumer
    {
        private readonly Func<string> folderPathCreator;

        private string folderPath;
        private bool isInitialized;

        public FileScreenshotConsumer(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public FileScreenshotConsumer(Func<string> folderPathCreator)
        {
            this.folderPathCreator = folderPathCreator;
        }

        /// <summary>
        /// Gets or sets the image format. The default format is Png.
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        private void Initialize()
        {
            if (folderPathCreator != null)
                folderPath = folderPathCreator();

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            isInitialized = true;
        }

        /// <summary>
        /// Takes the specified screenshot.
        /// </summary>
        /// <param name="screenshot">The screenshot.</param>
        /// <param name="number">The number of the screenshot.</param>
        /// <param name="title">The title. Can be null.</param>
        public void Take(Screenshot screenshot, int number, string title)
        {
            if (!isInitialized)
                Initialize();

            string fileName = $"{number:D2} {SanitizeFileName(title)}.{GetImageFormatExtension(ImageFormat)}";
            string filePath = Path.Combine(folderPath, fileName);

            screenshot.SaveAsFile(filePath, ImageFormat);
        }

        private string SanitizeFileName(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        private static string GetImageFormatExtension(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders().First(x => x.FormatID == format.Guid).FilenameExtension;
        }
    }
}
