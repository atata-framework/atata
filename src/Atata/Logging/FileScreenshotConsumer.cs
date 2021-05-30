using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the screenshot consumer that saves the screenshot to the file.
    /// By default uses <c>"Logs\{build-start}\{test-name-sanitized}"</c> as folder path format,
    /// <c>"{screenshot-number:D2} - {screenshot-pageobjectfullname}{screenshot-title: - *}"</c> as file name format
    /// and <see cref="OpenQA.Selenium.ScreenshotImageFormat.Png"/> as image format.
    /// Example of screenshot file path using default settings: <c>"Logs\2018-03-03 14_34_04\SampleTest\01 - Home page - Screenshot title.png"</c>.
    /// Available path variables are:
    /// <c>{build-start}</c>,
    /// <c>{test-name}</c>, <c>{test-name-sanitized}</c>,
    /// <c>{test-suite-name}</c>, <c>{test-suite-name-sanitized}</c>,
    /// <c>{test-start}</c>, <c>{driver-alias}</c>, <c>{screenshot-number}</c>,
    /// <c>{screenshot-title}</c>, <c>{screenshot-pageobjectname}</c>,
    /// <c>{screenshot-pageobjecttypename}</c>, <c>{screenshot-pageobjectfullname}</c>.
    /// Path variables support the formatting.
    /// </summary>
    /// <seealso cref="IScreenshotConsumer" />
    public class FileScreenshotConsumer : FileScreenshotConsumerBase
    {
        /// <summary>
        /// The default DateTime format for <c>"build-start"</c> and <c>"test-start"</c> path variables is <c>"yyyy-MM-dd HH_mm_ss"</c>.
        /// </summary>
        public const string DefaultDateTimeFormat = DefaultAtataContextArtifactsDirectory.DefaultDateTimeFormat;

        /// <summary>
        /// Gets or sets the builder of the folder path.
        /// </summary>
        public Func<string> FolderPathBuilder { get; set; }

        /// <summary>
        /// Gets or sets the builder of the file name.
        /// </summary>
        public Func<ScreenshotInfo, string> FileNameBuilder { get; set; }

        /// <summary>
        /// Gets or sets the builder of the file path.
        /// </summary>
        public Func<ScreenshotInfo, string> FilePathBuilder { get; set; }

        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Formats the screenshot file path using path variables.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="screenshotInfo">The screenshot information.</param>
        /// <returns>The formatted file path format.</returns>
        public static string FormatPath(string format, ScreenshotInfo screenshotInfo)
        {
            if (format.Contains('{'))
            {
                var screenshotVariables = new Dictionary<string, object>
                {
                    ["screenshot-number"] = screenshotInfo.Number,
                    ["screenshot-title"] = screenshotInfo.Title,
                    ["screenshot-pageobjectname"] = screenshotInfo.PageObjectName,
                    ["screenshot-pageobjecttypename"] = screenshotInfo.PageObjectTypeName,
                    ["screenshot-pageobjectfullname"] = screenshotInfo.PageObjectFullName
                };

                return AtataContext.Current.FillTemplateString(format, screenshotVariables);
            }
            else
            {
                return format;
            }
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
                    : BuildDefaultFolderPath());

            folderPath = folderPath.SanitizeForPath();

            string fileName = FileNameBuilder?.Invoke(screenshotInfo)
                ?? (!string.IsNullOrWhiteSpace(FileName)
                    ? FormatPath(FileName, screenshotInfo)
                    : BuildDefaultFileName(screenshotInfo));

            fileName = fileName.SanitizeForFileName();

            return Path.Combine(folderPath, fileName);
        }

        protected virtual string BuildDefaultFolderPath() =>
            DefaultAtataContextArtifactsDirectory.BuildPath();

        protected virtual string BuildDefaultFileName(ScreenshotInfo screenshotInfo) =>
            $"{screenshotInfo.Number:D2} - {screenshotInfo.PageObjectFullName}{screenshotInfo.Title?.Prepend(" - ")}";
    }
}
