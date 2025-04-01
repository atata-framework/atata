namespace Atata.UnitTests;

public class LogConsumersBuilderTests
{
    protected Subject<LogConsumersBuilder> Sut { get; private set; }

    [SetUp]
    public void SetUp() =>
        Sut = new LogConsumersBuilder(AtataContext.CreateDefaultNonScopedBuilder())
            .ToSutSubject();

    public sealed class Configure : LogConsumersBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Invoking(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn)))
                .Should.Throw<LogConsumerNotFoundException>("Failed to find TraceLogConsumer in AtataContextBuilder.");

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }

    public sealed class ConfigureIfExists : LogConsumersBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Act(x => x.ConfigureIfExists<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn)))
                .ResultOf(x => x.Items)
                    .Should.BeEmpty();

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.ConfigureIfExists<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }

    public sealed class ConfigureOrAdd : LogConsumersBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Act(x => x.ConfigureOrAdd<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenNewAndThereIsAnotherExisting() =>
            Sut.Act(x => x.Add<ConsoleLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.ConfigureOrAdd<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn)))
                .ResultOf(x => x.Items)
                    .Should.ConsistSequentiallyOf(
                        x => x.Consumer is ConsoleLogConsumer,
                        x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.ConfigureOrAdd<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }
}
