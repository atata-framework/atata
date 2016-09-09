using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

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
        public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

        protected static string SanitizeFileName(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        private static string GetImageFormatExtension(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders().
                First(x => x.FormatID == format.Guid).
                FilenameExtension.
                Split(';').
                First().
                TrimStart('*').
                ToLower();
        }

        private void Initialize()
        {
            if (folderPathCreator != null)
                folderPath = folderPathCreator();

            if (!Path.IsPathRooted(folderPath))
                folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderPath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            isInitialized = true;
        }

        /// <summary>
        /// Takes the specified screenshot.
        /// </summary>
        /// <param name="screenshotInfo">The screenshot information.</param>
        public void Take(ScreenshotInfo screenshotInfo)
        {
            if (!isInitialized)
                Initialize();

            string fileName = string.Concat(BuildFileName(screenshotInfo), GetImageFormatExtension(ImageFormat));
            string filePath = Path.Combine(folderPath, fileName);

            screenshotInfo.Screenshot.SaveAsFile(filePath, ImageFormat);
        }

        /// <summary>
        /// Builds the name of the file without the extension.
        /// </summary>
        /// <param name="screenshotInfo">The screenshot information.</param>
        /// <returns>The file name without the extension.</returns>
        protected virtual string BuildFileName(ScreenshotInfo screenshotInfo)
        {
            string fileName = $"{screenshotInfo.Number:D2} - {SanitizeFileName(screenshotInfo.PageObjectFullName)}";

            return string.IsNullOrWhiteSpace(screenshotInfo.Title)
                ? fileName
                : string.Concat(fileName, $" - {SanitizeFileName(screenshotInfo.Title)}");
        }
    }
}
