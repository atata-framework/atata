namespace Atata.UnitTests;

[TestFixture]
public class LogConsumersBuilderTests
{
    protected Subject<LogConsumersBuilder> Sut { get; private set; }

    [SetUp]
    public void SetUp() =>
        Sut = new LogConsumersBuilder()
            .ToSutSubject();

    public class Configure : LogConsumersBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Act(x => x.Configure<TraceLogConsumer>().WithMinLevel(LogLevel.Warn))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenNewAndThereIsAnotherExisting() =>
            Sut.Act(x => x.Add<ConsoleLogConsumer>().WithMinLevel(LogLevel.Info))
                .Act(x => x.Configure<TraceLogConsumer>().WithMinLevel(LogLevel.Warn))
                .ResultOf(x => x.Items)
                    .Should.ConsistSequentiallyOf(
                        x => x.Consumer is ConsoleLogConsumer,
                        x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>().WithMinLevel(LogLevel.Info))
                .Act(x => x.Configure<TraceLogConsumer>().WithMinLevel(LogLevel.Warn))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }
}
