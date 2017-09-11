using System.Linq;

namespace Atata
{
    public static class LogConsumerAtataContextBuilderExtensions
    {
        /// <summary>
        /// Defines that the logging should not use section-like pair messages (not "Starting: {action}" and "Finished: {action}", but just "{action}").
        /// </summary>
        /// <typeparam name="TTLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{TTLogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<TTLogConsumer> WithoutSectionFinish<TTLogConsumer>(this AtataContextBuilder<TTLogConsumer> builder)
            where TTLogConsumer : ILogConsumer
        {
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.LogSectionFinish = false;
            return builder;
        }

        /// <summary>
        /// Specifies the minimum level of the log event to write to the log.
        /// </summary>
        /// <typeparam name="TTLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="level">The level.</param>
        /// <returns>The <see cref="AtataContextBuilder{TTLogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<TTLogConsumer> WithMinLevel<TTLogConsumer>(this AtataContextBuilder<TTLogConsumer> builder, LogLevel level)
            where TTLogConsumer : ILogConsumer
        {
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.MinLevel = level;
            return builder;
        }
    }
}
