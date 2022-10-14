namespace Atata.UnitTests;

[TestFixture]
public class LogConsumersAtataContextBuilderTests
{
    protected Subject<LogConsumersAtataContextBuilder> Sut { get; private set; }

    [SetUp]
    public void SetUp() =>
        Sut = new LogConsumersAtataContextBuilder(new AtataBuildingContext())
            .ToSutSubject();

    public class Configure : LogConsumersAtataContextBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Act(x => x.Configure<TraceLogConsumer>().WithMinLevel(LogLevel.Warn))
                .ResultOf(x => x.BuildingContext.LogConsumerConfigurations)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenNewAndThereIsAnotherExisting() =>
            Sut.Act(x => x.Add<ConsoleLogConsumer>().WithMinLevel(LogLevel.Info))
                .Act(x => x.Configure<TraceLogConsumer>().WithMinLevel(LogLevel.Warn))
                .ResultOf(x => x.BuildingContext.LogConsumerConfigurations)
                    .Should.ConsistSequentiallyOf(
                        x => x.Consumer is ConsoleLogConsumer,
                        x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>().WithMinLevel(LogLevel.Info))
                .Act(x => x.Configure<TraceLogConsumer>().WithMinLevel(LogLevel.Warn))
                .ResultOf(x => x.BuildingContext.LogConsumerConfigurations)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }
}
