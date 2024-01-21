using System.Collections.ObjectModel;

namespace Atata.UnitTests;

[TestFixture]
public class LogManagerTests
{
    private LogManager _sut;

    private LogConsumerSpy _consumerSpy;

    [SetUp]
    public void SetUp()
    {
        _consumerSpy = new LogConsumerSpy();
        _sut = new LogManager(new BasicLogEventInfoFactory());
    }

    [Test]
    public void AddSecretStringsToMask()
    {
        _sut.AddConfiguration(new LogConsumerConfiguration(_consumerSpy));

        _sut.AddSecretStringsToMask(
            [new SecretStringToMask("abc123", "***")]);

        _sut.Info(@"Set ""abc123"" to something");

        _consumerSpy.CollectedEvents.Should.ContainSingle()
            .Single().ValueOf(x => x.Message).Should.Be(@"Set ""***"" to something");
    }

    [Test]
    public void WhenConsumerSectionEndIsExclude()
    {
        _sut.AddConfiguration(new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Exclude));

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo();

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.EqualSequence(
            "step section",
            "- trace sub-section",
            "- - inner info message",
            "- - inner trace message");
    }

    [Test]
    public void WhenConsumerSectionEndIsExclude_AndMinLogLevelIsInfo()
    {
        _sut.AddConfiguration(new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.Exclude));

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo();

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.EqualSequence(
            "step section",
            "- inner info message");
    }

    [Test]
    public void WhenConsumerSectionEndIsIncludeForBlocks()
    {
        _sut.AddConfiguration(new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.IncludeForBlocks));

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo();

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
        _sut.AddConfiguration(new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.IncludeForBlocks));

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo();

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void WhenConsumerSectionEndIsInclude()
    {
        _sut.AddConfiguration(new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Include));

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo();

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
        _sut.AddConfiguration(new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.Include));

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo();

        _consumerSpy.CollectedEvents.Select(x => x.Message).Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    private void LogStepSectionWithTraceSubSectionContainingTraceAndInfo() =>
        _sut.ExecuteSection(
            new StepLogSection("step section"),
            () => _sut.ExecuteSection(
                new LogSection("trace sub-section", LogLevel.Trace),
                () =>
                {
                    _sut.Info("inner info message");
                    _sut.Trace("inner trace message");
                }));

    public sealed class LogConsumerSpy : ILogConsumer
    {
        private readonly List<LogEventInfo> _collectedEvents = [];

        public Subject<ReadOnlyCollection<LogEventInfo>> CollectedEvents =>
            _collectedEvents.ToReadOnly().ToSubject(nameof(CollectedEvents));

        public void Log(LogEventInfo eventInfo) =>
            _collectedEvents.Add(eventInfo);
    }
}
