using System;
using OpenQA.Selenium;

namespace Atata
{
    public static class ScreenshotConsumerAtataContextBuilderExtensions
    {
        /// <summary>
        /// Specifies the image format of the file screenshot consumer.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the file screenshot consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<TConsumer> With<TConsumer>(
            this ScreenshotConsumerAtataContextBuilder<TConsumer> builder,
            ScreenshotImageFormat imageFormat)
            where TConsumer : FileScreenshotConsumerBase
        {
            builder.Context.ImageFormat = imageFormat;
            return builder;
        }

        [Obsolete("Use " + nameof(WithArtifactsDirectoryPath) + " instead.")] // Obsolete since v2.0.0.
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithArtifactsFolderPath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder)
            =>
            builder.WithArtifactsDirectoryPath();

        /// <summary>
        /// Sets the <see cref="AtataContext.Artifacts"/> directory as the directory path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithArtifactsDirectoryPath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder)
            =>
            builder.WithDirectoryPath(() => AtataContext.Current.Artifacts.FullName.Value);

        [Obsolete("Use " + nameof(WithDirectoryPath) + " instead.")] // Obsolete since v2.0.0.
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithFolderPath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            Func<string> folderPathBuilder)
            =>
            builder.WithDirectoryPath(folderPathBuilder);

        [Obsolete("Use " + nameof(WithDirectoryPath) + " instead.")] // Obsolete since v2.0.0.
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithFolderPath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            string folderPath)
            =>
            builder.WithDirectoryPath(folderPath);

        /// <summary>
        /// Specifies the directory path builder of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="directoryPathBuilder">The directory path builder function.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithDirectoryPath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            Func<string> directoryPathBuilder)
        {
            builder.Context.DirectoryPathBuilder = directoryPathBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the directory path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithDirectoryPath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            string directoryPath)
        {
            builder.Context.DirectoryPath = directoryPath;
            builder.Context.DirectoryPathBuilder = null;
            return builder;
        }

        /// <summary>
        /// Specifies the file name builder of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileNameBuilder">The file name builder function that takes an instance of <see cref="ScreenshotInfo"/>.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithFileName(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            Func<ScreenshotInfo, string> fileNameBuilder)
        {
            builder.Context.FileNameBuilder = fileNameBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the file name of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithFileName(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            string fileName)
        {
            builder.Context.FileName = fileName;
            builder.Context.FileNameBuilder = null;
            return builder;
        }

        /// <summary>
        /// Specifies the file path builder of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="filePathBuilder">The file path builder function that takes an instance of <see cref="ScreenshotInfo"/>.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithFilePath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            Func<ScreenshotInfo, string> filePathBuilder)
        {
            builder.Context.FilePathBuilder = filePathBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the file path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>The same builder instance.</returns>
        public static ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> WithFilePath(
            this ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> builder,
            string filePath)
        {
            builder.Context.FilePath = filePath;
            builder.Context.FilePathBuilder = null;
            return builder;
        }
    }
}
