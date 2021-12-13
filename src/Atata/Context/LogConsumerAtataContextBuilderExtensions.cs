using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class LogConsumerAtataContextBuilderExtensions
    {
        /// <summary>
        /// Defines that the logging should not use section-like pair messages (not <c>"Starting: {action}"</c> and <c>"Finished: {action}"</c>, but just <c>"{action}"</c>).
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TLogConsumer> WithoutSectionFinish<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder)
            where TLogConsumer : ILogConsumer
        {
            GetCurrentLogConsumerConfiguration(builder).LogSectionFinish = false;
            return builder;
        }

        /// <summary>
        /// Specifies the minimum level of the log event to write to the log.
        /// The default value is <see cref="LogLevel.Trace"/>.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="level">The level.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TLogConsumer> WithMinLevel<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder, LogLevel level)
            where TLogConsumer : ILogConsumer
        {
            GetCurrentLogConsumerConfiguration(builder).MinLevel = level;
            return builder;
        }

        /// <summary>
        /// Specifies the nesting level indent of the log message.
        /// The default value is <c>"- "</c>.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="messageNestingLevelIndent">The message nesting level indent.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TLogConsumer> WithMessageNestingLevelIndent<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder, string messageNestingLevelIndent)
            where TLogConsumer : ILogConsumer
        {
            GetCurrentLogConsumerConfiguration(builder).MessageNestingLevelIndent = messageNestingLevelIndent;
            return builder;
        }

        /// <summary>
        /// Specifies the start section prefix of the log message.
        /// The default value is <c>"&gt; "</c>.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="messageStartSectionPrefix">The message start section prefix.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TLogConsumer> WithMessageStartSectionPrefix<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder, string messageStartSectionPrefix)
            where TLogConsumer : ILogConsumer
        {
            GetCurrentLogConsumerConfiguration(builder).MessageStartSectionPrefix = messageStartSectionPrefix;
            return builder;
        }

        /// <summary>
        /// Specifies the end section prefix of the log message.
        /// The default value is <c>"&lt; "</c>.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="messageEndSectionPrefix">The message end section prefix.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TLogConsumer> WithMessageEndSectionPrefix<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder, string messageEndSectionPrefix)
            where TLogConsumer : ILogConsumer
        {
            GetCurrentLogConsumerConfiguration(builder).MessageEndSectionPrefix = messageEndSectionPrefix;
            return builder;
        }

        private static LogConsumerConfiguration GetCurrentLogConsumerConfiguration<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder)
            where TLogConsumer : ILogConsumer
        {
            return builder.BuildingContext.LogConsumerConfigurations.Single(x => Equals(x.Consumer, builder.Context));
        }

        /// <summary>
        /// Specifies the name of the logger.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="loggerName">The name of the logger.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TLogConsumer> WithLoggerName<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder, string loggerName)
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
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<Log4NetConsumer> WithRepositoryName(this AtataContextBuilder<Log4NetConsumer> builder, string repositoryName)
        {
            builder.Context.RepositoryName = repositoryName;
            return builder;
        }

        /// <summary>
        /// Specifies the assembly to use to lookup the logger repository.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="repositoryAssembly">The name of the assembly to use to lookup the repository.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<Log4NetConsumer> WithRepositoryAssembly(this AtataContextBuilder<Log4NetConsumer> builder, Assembly repositoryAssembly)
        {
            builder.Context.RepositoryAssembly = repositoryAssembly;
            return builder;
        }

        /// <summary>
        /// Specifies the full file path of the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithFilePath(this AtataContextBuilder<NLogFileConsumer> builder, string filePath)
        {
            filePath.CheckNotNullOrWhitespace(nameof(filePath));

            return builder.WithFilePath(_ => filePath);
        }

        /// <summary>
        /// Specifies the full file path builder for the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="filePathBuilder">The file path builder function.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithFilePath(this AtataContextBuilder<NLogFileConsumer> builder, Func<AtataContext, string> filePathBuilder)
        {
            builder.Context.FilePathBuilder = filePathBuilder;
            return builder;
        }

        /// <summary>
        /// Sets the <see cref="AtataContext"/> Artifacts folder as the folder path of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithArtifactsFolderPath(this AtataContextBuilder<NLogFileConsumer> builder) =>
            builder.WithFolderPath(x => x.Artifacts.FullName);

        /// <summary>
        /// Specifies the folder path of the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithFolderPath(this AtataContextBuilder<NLogFileConsumer> builder, string folderPath)
        {
            folderPath.CheckNotNullOrWhitespace(nameof(folderPath));

            return builder.WithFolderPath(_ => folderPath);
        }

        /// <summary>
        /// Specifies the folder path builder for the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPathBuilder">The folder path builder function.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithFolderPath(this AtataContextBuilder<NLogFileConsumer> builder, Func<AtataContext, string> folderPathBuilder)
        {
            builder.Context.FolderPathBuilder = folderPathBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the file name of the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileName">The file path.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithFileName(this AtataContextBuilder<NLogFileConsumer> builder, string fileName)
        {
            fileName.CheckNotNullOrWhitespace(nameof(fileName));

            return builder.WithFileName(_ => fileName);
        }

        /// <summary>
        /// Specifies the file name builder for the log file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="fileNameBuilder">The file path builder function.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithFileName(this AtataContextBuilder<NLogFileConsumer> builder, Func<AtataContext, string> fileNameBuilder)
        {
            builder.Context.FileNameBuilder = fileNameBuilder;
            return builder;
        }

        /// <summary>
        /// Specifies the layout of log event.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="layout">The layout of log event.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<NLogFileConsumer> WithLayout(this AtataContextBuilder<NLogFileConsumer> builder, string layout)
        {
            builder.Context.Layout = layout;
            return builder;
        }
    }
}
