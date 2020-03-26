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
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.LogSectionFinish = false;
            return builder;
        }

        /// <summary>
        /// Specifies the minimum level of the log event to write to the log.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="level">The level.</param>
        /// <returns>The <see cref="AtataContextBuilder{TContext}"/> instance.</returns>
        public static AtataContextBuilder<TLogConsumer> WithMinLevel<TLogConsumer>(this AtataContextBuilder<TLogConsumer> builder, LogLevel level)
            where TLogConsumer : ILogConsumer
        {
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.MinLevel = level;
            return builder;
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
    }
}
