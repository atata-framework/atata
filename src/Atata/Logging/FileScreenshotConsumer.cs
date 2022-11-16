using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents the screenshot consumer that saves the screenshot to the file.
    /// By default uses <see cref="AtataContext.Artifacts"/> directory as a directory path format,
    /// <c>"{screenshot-number:D2}{screenshot-pageobjectname: - *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}"</c> as a file name format
    /// and <see cref="OpenQA.Selenium.ScreenshotImageFormat.Png"/> as an image format.
    /// Example of a screenshot file path using default settings: <c>"artifacts\20220303T143404\SampleTest\01 - Home page - Screenshot title.png"</c>.
    /// Available predefined path variables are:
    /// <c>{build-start}</c>, <c>{build-start-utc}</c>
    /// <c>{test-name}</c>, <c>{test-name-sanitized}</c>,
    /// <c>{test-suite-name}</c>, <c>{test-suite-name-sanitized}</c>,
    /// <c>{test-start}</c>, <c>{test-start-utc}</c>,
    /// <c>{driver-alias}</c>, <c>{screenshot-number}</c>,
    /// <c>{screenshot-title}</c>, <c>{screenshot-pageobjectname}</c>,
    /// <c>{screenshot-pageobjecttypename}</c>, <c>{screenshot-pageobjectfullname}</c>.
    /// Path variables support formatting.
    /// </summary>
    public class FileScreenshotConsumer : FileScreenshotConsumerBase
    {
        [Obsolete("Use " + nameof(DirectoryPathBuilder) + " instead.")] // Obsolete since v2.0.0.
        public Func<string> FolderPathBuilder
        {
            get => DirectoryPathBuilder;
            set => DirectoryPathBuilder = value;
        }

        /// <summary>
        /// Gets or sets the builder of the directory path.
        /// </summary>
        public Func<string> DirectoryPathBuilder { get; set; }

        /// <summary>
        /// Gets or sets the builder of the file name.
        /// </summary>
        public Func<ScreenshotInfo, string> FileNameBuilder { get; set; }

        /// <summary>
        /// Gets or sets the builder of the file path.
        /// </summary>
        public Func<ScreenshotInfo, string> FilePathBuilder { get; set; }

        [Obsolete("Use " + nameof(DirectoryPath) + " instead.")] // Obsolete since v2.0.0.
        public string FolderPath
        {
            get => DirectoryPath;
            set => DirectoryPath = value;
        }

        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        public string DirectoryPath { get; set; }

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

            string directoryPath = DirectoryPathBuilder?.Invoke()
                ?? (!string.IsNullOrWhiteSpace(DirectoryPath)
                    ? FormatPath(DirectoryPath, screenshotInfo)
                    : BuildDefaultDirectoryPath());

            directoryPath = directoryPath.SanitizeForPath();

            string fileName = FileNameBuilder?.Invoke(screenshotInfo)
                ?? (!string.IsNullOrWhiteSpace(FileName)
                    ? FormatPath(FileName, screenshotInfo)
                    : BuildDefaultFileName(screenshotInfo));

            fileName = fileName.SanitizeForFileName();

            return Path.Combine(directoryPath, fileName);
        }

        protected virtual string BuildDefaultDirectoryPath() =>
            AtataContext.Current.Artifacts.FullName;

        protected virtual string BuildDefaultFileName(ScreenshotInfo screenshotInfo)
        {
            StringBuilder builder = new StringBuilder($"{screenshotInfo.Number:D2}");

            if (screenshotInfo.PageObjectName != null)
                builder.Append($" - {screenshotInfo.PageObjectName}");

            if (screenshotInfo.PageObjectTypeName != null)
                builder.Append($" {screenshotInfo.PageObjectTypeName}");

            if (screenshotInfo.Title != null)
                builder.Append($" - {screenshotInfo.Title}");

            return builder.ToString();
        }
    }
}
