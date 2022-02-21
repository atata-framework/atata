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
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TConsumer> With<TConsumer>(this AtataContextBuilder<TConsumer> builder, ScreenshotImageFormat imageFormat)
            where TConsumer : FileScreenshotConsumerBase
        {
            builder.Context.ImageFormat = imageFormat;
            return builder;
        }

        /// <summary>
        /// Sets the <see cref="AtataContext"/> Artifacts folder as the folder path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> WithArtifactsFolderPath(this AtataContextBuilder<FileScreenshotConsumer> builder) =>
            builder.WithFolderPath(() => AtataContext.Current.Artifacts.FullName.Value);

        /// <summary>
        /// Specifies the folder path builder of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPathBuilder">The folder path builder function.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> WithFolderPath(this AtataContextBuilder<FileScreenshotConsumer> builder, Func<string> folderPathBuilder)
        {
            builder.Context.FolderPathBuilder = folderPathBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the folder path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> WithFolderPath(this AtataContextBuilder<FileScreenshotConsumer> builder, string folderPath)
        {
            builder.Context.FolderPath = folderPath;
            builder.Context.FolderPathBuilder = null;
            return builder;
        }

        /// <summary>
        /// Specifies the file name builder of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileNameBuilder">The file name builder function that takes an instance of <see cref="ScreenshotInfo"/>.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> WithFileName(this AtataContextBuilder<FileScreenshotConsumer> builder, Func<ScreenshotInfo, string> fileNameBuilder)
        {
            builder.Context.FileNameBuilder = fileNameBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the file name of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> WithFileName(this AtataContextBuilder<FileScreenshotConsumer> builder, string fileName)
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
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> WithFilePath(this AtataContextBuilder<FileScreenshotConsumer> builder, Func<ScreenshotInfo, string> filePathBuilder)
        {
            builder.Context.FilePathBuilder = filePathBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the file path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> WithFilePath(this AtataContextBuilder<FileScreenshotConsumer> builder, string filePath)
        {
            builder.Context.FilePath = filePath;
            builder.Context.FilePathBuilder = null;
            return builder;
        }
    }
}
