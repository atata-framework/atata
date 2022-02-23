using System;
using System.Reflection;

namespace Atata
{
    public static class LogConsumerAtataContextBuilderExtensions
    {
        /// <summary>
        /// Specifies the name of the logger.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="loggerName">The name of the logger.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<TLogConsumer> WithLoggerName<TLogConsumer>(
            this LogConsumerAtataContextBuilder<TLogConsumer> builder,
            string loggerName)
            where TLogConsumer : INamedLogConsumer
        {
            builder.Context.LoggerName = loggerName;
            return builder;
        }

        /// <summary>
        /// Specifies the name of the logger repository.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="repositoryName">The name of the logger repository.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<Log4NetConsumer> WithRepositoryName(
            this LogConsumerAtataContextBuilder<Log4NetConsumer> builder,
            string repositoryName)
        {
            builder.Context.RepositoryName = repositoryName;
            return builder;
        }

        /// <summary>
        /// Specifies the assembly to use to lookup the logger repository.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="repositoryAssembly">The name of the assembly to use to lookup the repository.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<Log4NetConsumer> WithRepositoryAssembly(
            this LogConsumerAtataContextBuilder<Log4NetConsumer> builder,
            Assembly repositoryAssembly)
        {
            builder.Context.RepositoryAssembly = repositoryAssembly;
            return builder;
        }

        /// <summary>
        /// Specifies the full file path of the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithFilePath(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
            string filePath)
        {
            filePath.CheckNotNullOrWhitespace(nameof(filePath));

            return builder.WithFilePath(_ => filePath);
        }

        /// <summary>
        /// Specifies the full file path builder for the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="filePathBuilder">The file path builder function.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithFilePath(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
            Func<AtataContext, string> filePathBuilder)
        {
            builder.Context.FilePathBuilder = filePathBuilder;
            return builder;
        }

        /// <summary>
        /// Sets the <see cref="AtataContext"/> Artifacts folder as the folder path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithArtifactsFolderPath(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder)
            =>
            builder.WithFolderPath(x => x.Artifacts.FullName);

        /// <summary>
        /// Specifies the folder path of the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithFolderPath(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
            string folderPath)
        {
            folderPath.CheckNotNullOrWhitespace(nameof(folderPath));

            return builder.WithFolderPath(_ => folderPath);
        }

        /// <summary>
        /// Specifies the folder path builder for the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPathBuilder">The folder path builder function.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithFolderPath(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
            Func<AtataContext, string> folderPathBuilder)
        {
            builder.Context.FolderPathBuilder = folderPathBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the file name of the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileName">The file path.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithFileName(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
            string fileName)
        {
            fileName.CheckNotNullOrWhitespace(nameof(fileName));

            return builder.WithFileName(_ => fileName);
        }

        /// <summary>
        /// Specifies the file name builder for the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileNameBuilder">The file path builder function.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithFileName(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
            Func<AtataContext, string> fileNameBuilder)
        {
            builder.Context.FileNameBuilder = fileNameBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the layout of log event.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="layout">The layout of log event.</param>
        /// <returns>The same builder instance.</returns>
        public static LogConsumerAtataContextBuilder<NLogFileConsumer> WithLayout(
            this LogConsumerAtataContextBuilder<NLogFileConsumer> builder,
            string layout)
        {
            builder.Context.Layout = layout;
            return builder;
        }
    }
}
