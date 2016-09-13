using System;

namespace Atata
{
    public class AtataContextBuilder
    {
        internal AtataContextBuilder(AtataBuildingContext buildingContext)
        {
            BuildingContext = buildingContext;
        }

        public AtataBuildingContext BuildingContext { get; internal set; }

        public AtataContextBuilder<TLogConsumer> UseLogConsumer<TLogConsumer>(TLogConsumer consumer)
            where TLogConsumer : ILogConsumer
        {
            consumer.CheckNotNull(nameof(consumer));

            BuildingContext.LogConsumers.Add(new LogConsumerInfo(consumer));
            return new AtataContextBuilder<TLogConsumer>(consumer, BuildingContext);
        }

        public AtataContextBuilder<TLogConsumer> UseScreenshotConsumer<TLogConsumer>(TLogConsumer consumer)
            where TLogConsumer : IScreenshotConsumer
        {
            consumer.CheckNotNull(nameof(consumer));

            BuildingContext.ScreenshotConsumers.Add(consumer);
            return new AtataContextBuilder<TLogConsumer>(consumer, BuildingContext);
        }

        public AtataContextBuilder UseTestName(string name)
        {
            BuildingContext.TestName = name;
            return this;
        }

        public AtataContextBuilder UseBaseUrl(string url)
        {
            BuildingContext.BaseUrl = url;
            return this;
        }

        public void SetUp()
        {
            throw new NotImplementedException();
        }
    }
}
