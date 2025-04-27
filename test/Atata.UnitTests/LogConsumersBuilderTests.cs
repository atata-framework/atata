namespace Atata.UnitTests;

public class LogConsumersBuilderTests
{
    protected Subject<LogConsumersBuilder> Sut { get; private set; } = null!;

    [SetUp]
    public void SetUp() =>
        Sut = new LogConsumersBuilder(AtataContext.CreateDefaultNonScopedBuilder())
            .ToSutSubject();

    public sealed class Configure_WithConfigureOrThrowMode : LogConsumersBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Invoking(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn), ConfigurationMode.ConfigureOrThrow))
                .Should.Throw<LogConsumerNotFoundException>("Failed to find TraceLogConsumer in AtataContextBuilder.");

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn), ConfigurationMode.ConfigureOrThrow))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }

    public sealed class Configure_WithConfigureIfExistsMode : LogConsumersBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Act(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn), ConfigurationMode.ConfigureIfExists))
                .ResultOf(x => x.Items)
                    .Should.BeEmpty();

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn), ConfigurationMode.ConfigureIfExists))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }

    public sealed class Configure_WithConfigureOrAddMode : LogConsumersBuilderTests
    {
        [Test]
        public void WhenNew() =>
            Sut.Act(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn), ConfigurationMode.ConfigureOrAdd))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenNewAndThereIsAnotherExisting() =>
            Sut.Act(x => x.Add<ConsoleLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn), ConfigurationMode.ConfigureOrAdd))
                .ResultOf(x => x.Items)
                    .Should.ConsistSequentiallyOf(
                        x => x.Consumer is ConsoleLogConsumer,
                        x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);

        [Test]
        public void WhenExisting() =>
            Sut.Act(x => x.Add<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Info)))
                .Act(x => x.Configure<TraceLogConsumer>(x => x.WithMinLevel(LogLevel.Warn), ConfigurationMode.ConfigureOrAdd))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(x => x.Consumer is TraceLogConsumer && x.MinLevel == LogLevel.Warn);
    }
}
