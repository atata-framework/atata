using System.Linq;

namespace Atata
{
    public static class AtataContextBuilderExtensions
    {
        public static AtataContextBuilder UseNUnitTestName(this AtataContextBuilder builder)
        {
            return builder.UseTestName(null);
        }

        public static AtataContextBuilder<ILogConsumer> UseTraceLogging(this AtataContextBuilder builder)
        {
            return builder.UseLogConsumer<ILogConsumer>(new TraceLogConsumer());
        }

        public static AtataContextBuilder<ILogConsumer> UseDebugLogging(this AtataContextBuilder builder)
        {
            return builder.UseLogConsumer<ILogConsumer>(new DebugLogConsumer());
        }

        public static AtataContextBuilder<ILogConsumer> UseNUnitTestContextLogging(this AtataContextBuilder builder)
        {
            return builder.UseLogConsumer<ILogConsumer>(new NUnitTestContextLogConsumer());
        }

        public static AtataContextBuilder<ILogConsumer> UseNLogLogging(this AtataContextBuilder builder, string loggerName = null)
        {
            return builder.UseLogConsumer<ILogConsumer>(new NLogConsumer(loggerName));
        }

        public static AtataContextBuilder<T> WithoutSectionFinish<T>(this AtataContextBuilder<T> builder)
            where T : ILogConsumer
        {
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.LogSectionFinish = false;
            return builder;
        }

        public static AtataContextBuilder<T> WithMinLevel<T>(this AtataContextBuilder<T> builder, LogLevel level)
            where T : ILogConsumer
        {
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.MinLevel = level;
            return builder;
        }
    }
}
