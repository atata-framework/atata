using System.Collections.ObjectModel;

namespace Atata.UnitTests;

[TestFixture]
public class LogManagerTests
{
    private LogConsumerSpy _consumerSpy;

    [SetUp]
    public void SetUp() =>
        _consumerSpy = new LogConsumerSpy();

    [Test]
    public void Info_WithSecretString()
    {
        var sut = CreateSut(
            [new LogConsumerConfiguration(_consumerSpy)],
            [new SecretStringToMask("abc123", "***")]);

        sut.Info(@"Set ""abc123"" to something");

        _consumerSpy.CollectedEvents.Should.ContainSingle()
            .Single().ValueOf(x => x.Message).Should.Be(@"Set ""***"" to something");
    }

    [Test]
    public async Task ExecuteSectionAsync()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);

        await sut.ExecuteSectionAsync(
            new StepLogSection("step section"),
            async () => await sut.ExecuteSectionAsync(
                new LogSection("trace sub-section", LogLevel.Trace),
                () =>
                {
                    sut.Trace("inner trace message");
                    return Task.CompletedTask;
                }));

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- > trace sub-section",
            x => x == "- - inner trace message",
            x => x.StartsWith("- < trace sub-section ("),
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public async Task ExecuteSectionAsync_WithResult()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);

        var result = (await sut.ExecuteSectionAsync(
            new StepLogSection("step section"),
            async () => await sut.ExecuteSectionAsync(
                new LogSection("trace sub-section", LogLevel.Trace),
                () =>
                {
                    sut.Trace("inner trace message");
                    return Task.FromResult("ok");
                }))).ToResultSubject();

        result.Should.Be("ok");
        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- > trace sub-section",
            x => x == "- - inner trace message",
            x => x.StartsWith("- < trace sub-section ("),
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void WhenConsumerSectionEndIsExclude()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Exclude)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.EqualSequence(
            "step section",
            "- trace sub-section",
            "- - inner info message",
            "- - inner trace message");
    }

    [Test]
    public void WhenConsumerSectionEndIsExclude_AndMinLogLevelIsInfo()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.Exclude)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.EqualSequence(
            "step section",
            "- inner info message");
    }

    [Test]
    public void WhenConsumerSectionEndIsIncludeForBlocks()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.IncludeForBlocks)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- trace sub-section",
            x => x == "- - inner info message",
            x => x == "- - inner trace message",
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void WhenConsumerSectionEndIsIncludeForBlocks_AndMinLogLevelIsInfo()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.IncludeForBlocks)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void WhenConsumerSectionEndIsInclude()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Include)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- > trace sub-section",
            x => x == "- - inner info message",
            x => x == "- - inner trace message",
            x => x.StartsWith("- < trace sub-section ("),
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void WhenConsumerSectionEndIsInclude_AndMinLogLevelIsInfo()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.Include)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    private static LogManager CreateSut(
        LogConsumerConfiguration[] consumerConfigurations,
        SecretStringToMask[] secretStringsToMask = null) =>
        new(
            new(
                consumerConfigurations,
                secretStringsToMask ?? []),
            new BasicLogEventInfoFactory());

    private static void LogStepSectionWithTraceSubSectionContainingTraceAndInfo(LogManager sut) =>
        sut.ExecuteSection(
            new StepLogSection("step section"),
            () => sut.ExecuteSection(
                new LogSection("trace sub-section", LogLevel.Trace),
                () =>
                {
                    sut.Info("inner info message");
                    sut.Trace("inner trace message");
                }));

    private sealed class BasicLogEventInfoFactory : ILogEventInfoFactory
    {
        public LogEventInfo Create(LogLevel level, string message) =>
            new()
            {
                Level = level,
                Message = message,

                Timestamp = DateTime.Now
            };
    }

    public sealed class LogConsumerSpy : ILogConsumer
    {
        private readonly List<LogEventInfo> _collectedEvents = [];

        public Subject<ReadOnlyCollection<LogEventInfo>> CollectedEvents =>
            _collectedEvents.ToReadOnly().ToSubject(nameof(CollectedEvents));

        public void Log(LogEventInfo eventInfo) =>
            _collectedEvents.Add(eventInfo);
    }
}
