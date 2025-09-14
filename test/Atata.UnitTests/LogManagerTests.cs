namespace Atata.UnitTests;

public sealed class LogManagerTests
{
    private LogConsumerSpy _consumerSpy = null!;

    [SetUp]
    public void SetUp() =>
        _consumerSpy = new();

    [Test]
    public void Info_WithSecretString()
    {
        using var sut = CreateSut(
            [new LogConsumerConfiguration(_consumerSpy)],
            [new SecretStringToMask("abc123", "***")]);

        sut.Info(@"Set ""abc123"" to something");

        _consumerSpy.CollectedEvents.Should.ContainSingle()
            .Single().ValueOf(x => x.Message).Should.Be(@"Set ""***"" to something");
    }

    [Test]
    public async Task ExecuteSectionAsync()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);

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
            x => WildcardPattern.IsMatch(x, "- < trace sub-section (*)"),
            x => WildcardPattern.IsMatch(x, "< step section (*)"));
    }

    [Test]
    public async Task ExecuteSectionAsync_WithResult()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);

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
            x => WildcardPattern.IsMatch(x, "- < trace sub-section (*) >> \"ok\""),
            x => WildcardPattern.IsMatch(x, "< step section (*)"));
    }

    [Test]
    public void WhenConsumerSectionEndIsExclude()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Exclude)]);

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
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.Exclude)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.EqualSequence(
            "step section",
            "- inner info message");
    }

    [Test]
    public void WhenConsumerSectionEndIsIncludeForBlocks()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.IncludeForBlocks)]);

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
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.IncludeForBlocks)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void WhenConsumerSectionEndIsInclude()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Include)]);

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
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy, LogLevel.Info, LogSectionEndOption.Include)]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.ConsistSequentiallyOf(
            x => x == "> step section",
            x => x == "- inner info message",
            x => x.StartsWith("< step section ("));
    }

    [Test]
    public void ForCategory()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);
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
            x => x.NestingText == "- - < " && x.Message!.StartsWith("trace sub-section (") && x.Category == category1Name,
            x => x.NestingText == "- < " && x.Message!.StartsWith("step section (") && x.Category == category1Name,
            x => x.NestingText == "- " && x.Message == "trace for sub-category" && x.Category == category2Name,
            x => x.NestingText == "< " && x.Message!.StartsWith("root section (") && x.Category == null);
    }

    [Test]
    public void ForSource_WhenConsumerEmbedSourceLogIsTrue()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy) { EmbedSourceLog = true }]);
        const string sourceName = "src1";

        sut.ExecuteSection(
            "root section",
            () =>
            {
                var subSut1 = sut.ForSource(sourceName);
                LogStepSectionWithTraceSubSectionContainingTraceAndInfo(subSut1);

                var subSut2 = subSut1.ForCategory("cat1");
                subSut2.Trace("trace for sub-category");
            });

        _consumerSpy.CollectedEvents.Should.ConsistSequentiallyOf(
            x => x.NestingText == "> " && x.Message == "root section" && x.Source == null,
            x => x.NestingText == "- > " && x.Message == "step section" && x.Source == sourceName,
            x => x.NestingText == "- - > " && x.Message == "trace sub-section" && x.Source == sourceName,
            x => x.NestingText == "- - - " && x.Message == "inner info message" && x.Source == sourceName,
            x => x.NestingText == "- - - " && x.Message == "inner trace message" && x.Source == sourceName,
            x => x.NestingText == "- - < " && x.Message!.StartsWith("trace sub-section (") && x.Source == sourceName,
            x => x.NestingText == "- < " && x.Message!.StartsWith("step section (") && x.Source == sourceName,
            x => x.NestingText == "- " && x.Message == "trace for sub-category" && x.Source == sourceName && x.Category == "cat1",
            x => x.NestingText == "< " && x.Message!.StartsWith("root section (") && x.Source == null);
    }

    [Test]
    public void ForSource_WhenConsumerEmbedSourceLogIsFalse()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy) { EmbedSourceLog = false }]);
        const string sourceName = "src1";

        sut.ExecuteSection(
            "root section",
            () =>
            {
                var subSut1 = sut.ForSource(sourceName);
                LogStepSectionWithTraceSubSectionContainingTraceAndInfo(subSut1);

                subSut1.ForCategory("cat1")
                    .Trace("trace ext");

                sut.ForCategory("cat2")
                   .Trace("trace non-ext");
            });

        _consumerSpy.CollectedEvents.Should.ConsistSequentiallyOf(
            x => x.NestingText == "> " && x.Message == "root section" && x.Source == null,
            x => x.NestingText == "> " && x.Message == "step section" && x.Source == sourceName,
            x => x.NestingText == "- > " && x.Message == "trace sub-section" && x.Source == sourceName,
            x => x.NestingText == "- - " && x.Message == "inner info message" && x.Source == sourceName,
            x => x.NestingText == "- - " && x.Message == "inner trace message" && x.Source == sourceName,
            x => x.NestingText == "- < " && x.Message!.StartsWith("trace sub-section (") && x.Source == sourceName,
            x => x.NestingText == "< " && x.Message!.StartsWith("step section (") && x.Source == sourceName,
            x => x.NestingText == null && x.Message == "trace ext" && x.Source == sourceName && x.Category == "cat1",
            x => x.NestingText == "- " && x.Message == "trace non-ext" && x.Source == null && x.Category == "cat2",
            x => x.NestingText == "< " && x.Message!.StartsWith("root section (") && x.Source == null);
    }

    [Test]
    public void CreateSubLogForCategory()
    {
        using var sut = CreateSut([new LogConsumerConfiguration(_consumerSpy)]);
        const string category1Name = "cat1";
        const string category2Name = "cat2";

        sut.ExecuteSection(
            "root section",
            () =>
            {
                var subSut1 = sut.CreateSubLogForCategory(category1Name);
                var subSut2 = sut.ForCategory(category2Name);

                subSut1.ExecuteSection(
                    new StepLogSection("CreateSubLogForCategory"),
                    () =>
                    subSut2.ExecuteSection(
                        new LogSection("ForCategory", LogLevel.Trace),
                        () =>
                        {
                            subSut1.Trace("CreateSubLogForCategory trace message");
                            subSut2.Trace("ForCategory trace message");
                            sut.Trace("Root trace message");
                        }));
            });

        _consumerSpy.CollectedEvents.Should.ConsistSequentiallyOf(
            x => x.NestingText == "> " && x.Message == "root section" && x.Category == null,
            x => x.NestingText == "- > " && x.Message == "CreateSubLogForCategory" && x.Category == category1Name,
            x => x.NestingText == "- > " && x.Message == "ForCategory" && x.Category == category2Name,
            x => x.NestingText == "- - " && x.Message == "CreateSubLogForCategory trace message" && x.Category == category1Name,
            x => x.NestingText == "- - " && x.Message == "ForCategory trace message" && x.Category == category2Name,
            x => x.NestingText == "- " && x.Message == "Root trace message" && x.Category == null,
            x => x.NestingText == "- < " && x.Message!.StartsWith("ForCategory (") && x.Category == category2Name,
            x => x.NestingText == "- < " && x.Message!.StartsWith("CreateSubLogForCategory (") && x.Category == category1Name,
            x => x.NestingText == "< " && x.Message!.StartsWith("root section (") && x.Category == null);
    }

    [TestCase(TestResultStatusCondition.Passed)]
    [TestCase(TestResultStatusCondition.PassedOrInconclusive)]
    [TestCase(TestResultStatusCondition.PassedOrInconclusiveOrWarning)]
    public void TryReleasePostponingConsumers_WithFailedStatus_WhenConsumerSkipConditionIs(TestResultStatusCondition condition)
    {
        // Arrange
        using var sut = CreateSut(
            [
                new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Exclude)
                {
                    SkipCondition = condition
                }
            ]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.BeEmpty();

        // Act
        sut.TryReleasePostponingConsumers(TestResultStatus.Failed);

        // Assert
        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.EqualSequence(
            "step section",
            "- trace sub-section",
            "- - inner info message",
            "- - inner trace message");
    }

    [TestCase(TestResultStatusCondition.Passed)]
    [TestCase(TestResultStatusCondition.PassedOrInconclusive)]
    public void TryReleasePostponingConsumers_WithWarningStatus_WhenConsumerSkipConditionIs(TestResultStatusCondition condition)
    {
        // Arrange
        using var sut = CreateSut(
            [
                new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Exclude)
                {
                    SkipCondition = condition
                }
            ]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.BeEmpty();

        // Act
        sut.TryReleasePostponingConsumers(TestResultStatus.Warning);

        // Assert
        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.EqualSequence(
            "step section",
            "- trace sub-section",
            "- - inner info message",
            "- - inner trace message");
    }

    [Test]
    public void TryReleasePostponingConsumers_WithWarningStatus_WhenConsumerSkipConditionIsPassedOrInconclusiveOrWarning()
    {
        // Arrange
        using var sut = CreateSut(
            [
                new LogConsumerConfiguration(_consumerSpy, LogSectionEndOption.Exclude)
                {
                    SkipCondition = TestResultStatusCondition.PassedOrInconclusiveOrWarning
                }
            ]);

        LogStepSectionWithTraceSubSectionContainingTraceAndInfo(sut);

        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.BeEmpty();

        // Act
        sut.TryReleasePostponingConsumers(TestResultStatus.Warning);

        // Assert
        _consumerSpy.CollectedEventNestedTextsWithMessages.Should.BeEmpty();
    }

    private static LogManager CreateSut(
        LogConsumerConfiguration[] consumerConfigurations,
        SecretStringToMask[]? secretStringsToMask = null) =>
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
        public LogEventInfo Create(DateTime timestamp, LogLevel level, string? message) =>
            new(null!)
            {
                Timestamp = timestamp,
                Level = level,
                Message = message
            };
    }

    public sealed class LogConsumerSpy : ILogConsumer
    {
        private readonly List<LogEventInfo> _collectedEvents = [];

        public Subject<IReadOnlyList<LogEventInfo>> CollectedEvents =>
            new(_collectedEvents, nameof(CollectedEvents));

        public Subject<IReadOnlyList<string>> CollectedEventNestedTextsWithMessages =>
            new([.. _collectedEvents.Select(x => x.NestingText + x.Message)], nameof(CollectedEventNestedTextsWithMessages));

        public void Log(LogEventInfo eventInfo) =>
            _collectedEvents.Add(eventInfo);
    }
}
