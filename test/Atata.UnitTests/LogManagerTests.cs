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

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
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
        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
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

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.EqualSequence(
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

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.EqualSequence(
            "step section",
            "- inner info message");
    }

    [Test]
    public void WhenConsumerSectionEndIsIncludeForBlocks()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.IncludeForBlocks)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
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

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void WhenConsumerSectionEndIsInclude()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Include)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
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

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void ForCategory()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);
        const string category1Name = "cat1";
        const string category2Name = "cat2";

        sut.ExecuteSection(
            "root section",
            () =>
            {
                var subSut1 = sut.ForCategory(category1Name);
                LogStepSectionWithTraceSubSectionContainingTraceAndInfo(subSut1);

                var subSut2 = subSut1.ForCategory(category2Name);
                subSut2.Trace("trace for sub-category");
            });

        _consumerSpy.CollectedEvents.Should.ConsistSequentiallyOf(
            x => x.NestingText == "> " && x.Message == "root section" && x.Category == null,
            x => x.NestingText == "- > " && x.Message == "step section" && x.Category == category1Name,
            x => x.NestingText == "- - > " && x.Message == "trace sub-section" && x.Category == category1Name,
            x => x.NestingText == "- - - " && x.Message == "inner info message" && x.Category == category1Name,
            x => x.NestingText == "- - - " && x.Message == "inner trace message" && x.Category == category1Name,
            x => x.NestingText == "- - < " && x.Message.StartsWith("trace sub-section (") && x.Category == category1Name,
            x => x.NestingText == "- < " && x.Message.StartsWith("step section (") && x.Category == category1Name,
            x => x.NestingText == "- " && x.Message == "trace for sub-category" && x.Category == category2Name,
            x => x.NestingText == "< " && x.Message.StartsWith("root section (") && x.Category == null);
    }

    [Test]
    public void ForExternalSource()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);
        const string sourceName = "src1";

        sut.ExecuteSection(
            "root section",
            () =>
            {
                var subSut1 = sut.ForExternalSource(sourceName);
                LogStepSectionWithTraceSubSectionContainingTraceAndInfo(subSut1);

                var subSut2 = subSut1.ForCategory("cat1");
                subSut2.Trace("trace for sub-category");
            });

        _consumerSpy.CollectedEvents.Should.ConsistSequentiallyOf(
            x => x.NestingText == "> " && x.Message == "root section" && x.ExternalSource == null,
            x => x.NestingText == "- > " && x.Message == "step section" && x.ExternalSource == sourceName,
            x => x.NestingText == "- - > " && x.Message == "trace sub-section" && x.ExternalSource == sourceName,
            x => x.NestingText == "- - - " && x.Message == "inner info message" && x.ExternalSource == sourceName,
            x => x.NestingText == "- - - " && x.Message == "inner trace message" && x.ExternalSource == sourceName,
            x => x.NestingText == "- - < " && x.Message.StartsWith("trace sub-section (") && x.ExternalSource == sourceName,
            x => x.NestingText == "- < " && x.Message.StartsWith("step section (") && x.ExternalSource == sourceName,
            x => x.NestingText == "- " && x.Message == "trace for sub-category" && x.ExternalSource == sourceName && x.Category == "cat1",
            x => x.NestingText == "< " && x.Message.StartsWith("root section (") && x.ExternalSource == null);
    }

    [Test]
    public void ForExternalSource_WhenConsumerKeepHierarchyForExternalSourceIsFalse()
    {
        var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy) { EmbedExternalSourceLog = false }]);
        const string sourceName = "src1";

        sut.ExecuteSection(
            "root section",
            () =>
            {
                var subSut1 = sut.ForExternalSource(sourceName);
                LogStepSectionWithTraceSubSectionContainingTraceAndInfo(subSut1);

                subSut1.ForCategory("cat1")
                    .Trace("trace ext");

                sut.ForCategory("cat2")
                   .Trace("trace non-ext");
            });

        _consumerSpy.CollectedEvents.Should.ConsistSequentiallyOf(
            x => x.NestingText == "> " && x.Message == "root section" && x.ExternalSource == null,
            x => x.NestingText == "> " && x.Message == "step section" && x.ExternalSource == sourceName,
            x => x.NestingText == "- > " && x.Message == "trace sub-section" && x.ExternalSource == sourceName,
            x => x.NestingText == "- - " && x.Message == "inner info message" && x.ExternalSource == sourceName,
            x => x.NestingText == "- - " && x.Message == "inner trace message" && x.ExternalSource == sourceName,
            x => x.NestingText == "- < " && x.Message.StartsWith("trace sub-section (") && x.ExternalSource == sourceName,
            x => x.NestingText == "< " && x.Message.StartsWith("step section (") && x.ExternalSource == sourceName,
            x => x.NestingText == null && x.Message == "trace ext" && x.ExternalSource == sourceName && x.Category == "cat1",
            x => x.NestingText == "- " && x.Message == "trace non-ext" && x.ExternalSource == null && x.Category == "cat2",
            x => x.NestingText == "< " && x.Message.StartsWith("root section (") && x.ExternalSource == null);
    }

    private static LogManager CreateSut(
        LogConsumerConfiguration[] consumerConfigurations,
        SecretStringToMask[] secretStringsToMask = null) =>
        new(
            new(
                consumerConfigurations,
                secretStringsToMask ?? []),
            new BasicLogEventInfoFactory());

    private static void LogStepSectionWithTraceSubSectionContainingTraceAndInfo(ILogManager sut) =>
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
        public LogEventInfo Create(DateTime timestamp, LogLevel level, string message) =>
            new()
            {
                Timestamp = timestamp,
                Level = level,
                Message = message
            };
    }

    public sealed class LogConsumerSpy : ILogConsumer
    {
        private readonly List<LogEventInfo> _collectedEvents = [];

        public Subject<ReadOnlyCollection<LogEventInfo>> CollectedEvents =>
            _collectedEvents.ToReadOnly().ToSubject(nameof(CollectedEvents));

        public Subject<ReadOnlyCollection<string>> CollectedEventNestedTextsWithMessages =>
            _collectedEvents.Select(x => x.NestingText + x.Message).ToReadOnly().ToSubject(nameof(CollectedEventNestedTextsWithMessages));

        public void Log(LogEventInfo eventInfo) =>
            _collectedEvents.Add(eventInfo);
    }
}
