using System;
using System.IO;

namespace Atata
{
    /// <summary>
    /// Represents the screenshot consumer that saves the screenshot to the file.
    /// </summary>
    /// <seealso cref="Atata.IScreenshotConsumer" />
    public class FileScreenshotConsumer : FileScreenshotConsumerBase
    {
        public Func<string> FolderPathBuilder { get; set; }

        public Func<ScreenshotInfo, string> FileNameBuilder { get; set; }

        public Func<ScreenshotInfo, string> FilePathBuilder { get; set; }

        /// <summary>
        /// Builds the path of the file without the extension.
        /// </summary>
        /// <param name="screenshotInfo">The screenshot information.</param>
        /// <returns>The file path without the extension.</returns>
        protected override string BuildFilePath(ScreenshotInfo screenshotInfo)
        {
            if (FilePathBuilder != null)
                return FilePathBuilder(screenshotInfo);

            string folderPath = FolderPathBuilder?.Invoke()
                ?? $@"Logs\{AtataContext.BuildStart:yyyy-MM-dd HH_mm_ss}\{AtataContext.Current.TestName}";

            folderPath = folderPath.SanitizeForPath();

            string fileName = FileNameBuilder?.Invoke(screenshotInfo)
                ?? $"{screenshotInfo.Number:D2} - {screenshotInfo.PageObjectFullName}{screenshotInfo.Title?.Prepend(" - ")}";

            fileName = fileName.SanitizeForFileName();

            return Path.Combine(folderPath, fileName);
        }
    }
}
