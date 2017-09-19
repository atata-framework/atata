using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the screenshot consumer that saves the screenshot to the file.
    /// </summary>
    /// <seealso cref="Atata.IScreenshotConsumer" />
    public class FileScreenshotConsumer : FileScreenshotConsumerBase
    {
        private static readonly Dictionary<string, Func<ScreenshotInfo, object>> PathVariableFactoryMap;

        static FileScreenshotConsumer()
        {
            PathVariableFactoryMap = new Dictionary<string, Func<ScreenshotInfo, object>>
            {
                ["build-start"] = scr => AtataContext.BuildStart,

                ["test-name"] = scr => AtataContext.Current.TestName,
                ["test-start"] = scr => AtataContext.Current.TestStart,

                ["driver-alias"] = scr => AtataContext.Current.DriverAlias,

                ["screenshot-number"] = scr => scr.Number,
                ["screenshot-title"] = scr => scr.Title,
                ["screenshot-pageobjectname"] = scr => scr.PageObjectName,
                ["screenshot-pageobjecttypename"] = scr => scr.PageObjectTypeName,
                ["screenshot-pageobjectfullname"] = scr => scr.PageObjectFullName
            };
        }

        public Func<string> FolderPathBuilder { get; set; }

        public Func<ScreenshotInfo, string> FileNameBuilder { get; set; }

        public Func<ScreenshotInfo, string> FilePathBuilder { get; set; }

        public string FolderPath { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public static string FormatPath(string format, ScreenshotInfo screenshotInfo)
        {
            for (int i = 0; i < PathVariableFactoryMap.Count; i++)
            {
                format = format.Replace("{" + PathVariableFactoryMap.Keys.ElementAt(i), $"{{{i}}}");
            }

            return format.FormatWith(PathVariableFactoryMap.Values.Select(factory => factory(screenshotInfo)).ToArray());
        }

        /// <summary>
        /// Builds the path of the file without the extension.
        /// </summary>
        /// <param name="screenshotInfo">The screenshot information.</param>
        /// <returns>The file path without the extension.</returns>
        protected override string BuildFilePath(ScreenshotInfo screenshotInfo)
        {
            if (FilePathBuilder != null)
                return FilePathBuilder(screenshotInfo).SanitizeForPath();
            else if (!string.IsNullOrWhiteSpace(FilePath))
                return FormatPath(FilePath, screenshotInfo).SanitizeForPath();

            string folderPath = FolderPathBuilder?.Invoke()
                ?? (!string.IsNullOrWhiteSpace(FolderPath)
                    ? FormatPath(FolderPath, screenshotInfo)
                    : $@"Logs\{AtataContext.BuildStart:yyyy-MM-dd HH_mm_ss}\{AtataContext.Current.TestName}");

            folderPath = folderPath.SanitizeForPath();

            string fileName = FileNameBuilder?.Invoke(screenshotInfo)
                ?? (!string.IsNullOrWhiteSpace(FileName)
                    ? FormatPath(FileName, screenshotInfo)
                    : $"{screenshotInfo.Number:D2} - {screenshotInfo.PageObjectFullName}{screenshotInfo.Title?.Prepend(" - ")}");

            fileName = fileName.SanitizeForFileName();

            return Path.Combine(folderPath, fileName);
        }
    }
}
