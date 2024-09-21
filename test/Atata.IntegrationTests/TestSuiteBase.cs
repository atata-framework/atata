namespace Atata.IntegrationTests;

[TestFixture]
public abstract class TestSuiteBase
{
    private EventListLogConsumer _eventListLogConsumer;

    // TODO: Replace with method CurrentLog.GetSnapshot().
    protected IReadOnlyList<LogEventInfo> LogEntries =>
        _eventListLogConsumer.Items;

    protected AtataContextBuilder ConfigureSessionlessAtataContext()
    {
        _eventListLogConsumer = new EventListLogConsumer();

        var builder = AtataContext.Configure()
            .UseCulture("en-US")
            .UseNUnitTestName()
            .UseNUnitTestSuiteName()
            .UseNUnitTestSuiteType();

        builder.LogConsumers.AddNUnitTestContext();
        builder.LogConsumers.Add(_eventListLogConsumer)
            .WithMessageNestingLevelIndent(string.Empty);

        builder.EventSubscriptions.LogNUnitError();
        builder.EventSubscriptions.AddArtifactsToNUnitTestContext();

        return builder;
    }

    protected AtataContextBuilder ConfigureAtataContextWithFakeSession()
    {
        var builder = ConfigureSessionlessAtataContext();
        builder.Sessions.Add<FakeSessionBuilder>();
        return builder;
    }

    [TearDown]
    public virtual void TearDown() =>
        AtataContext.Current?.Dispose();

    protected static void VerifyEquals<T, TPage>(Field<T, TPage> control, T value)
        where TPage : PageObject<TPage>
    {
        control.Should.Equal(value);
        Assert.That(control.Value, Is.EqualTo(value));
    }

    protected static void VerifyDoesNotEqual<T, TPage>(Field<T, TPage> control, T value)
        where TPage : PageObject<TPage>
    {
        control.Should.Not.Equal(value);

        Assert.Throws<AssertionException>(() =>
            control.Should.AtOnce.Equal(value));
    }

    protected static TException AssertThrowsWithInnerException<TException, TInnerException>(TestDelegate code)
        where TException : Exception
        where TInnerException : Exception
    {
        TException exception = Assert.Throws<TException>(code);

        Assert.That(exception.InnerException, Is.InstanceOf<TInnerException>(), "Invalid inner exception.");

        return exception;
    }

    protected static TException AssertThrowsWithoutInnerException<TException>(TestDelegate code)
        where TException : Exception
    {
        TException exception = Assert.Throws<TException>(code);

        Assert.That(exception.InnerException, Is.Null, "Inner exception should be null.");

        return exception;
    }

    protected static void AssertThatFileExists(string filePath) =>
        Assert.That(new FileInfo(filePath), Does.Exist);

    protected static void AssertThatFileShouldContainText(string filePath, params string[] texts)
    {
        AssertThatFileExists(filePath);

        using FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader reader = new(fileStream);

        string fileContent = reader.ReadToEnd();
        fileContent.Should().ContainAll(texts);
    }

    protected static void AssertThatFileShouldNotContainText(string filePath, params string[] texts)
    {
        AssertThatFileExists(filePath);

        string fileContent = File.ReadAllText(filePath);
        fileContent.Should().NotContainAll(texts);
    }

    protected void VerifyLastLogMessages(LogLevel minLogLevel, params string[] expectedMessages)
    {
        string[] actualMessages = GetLastLogMessages(minLogLevel, expectedMessages.Length);

        Assert.That(actualMessages, Is.EqualTo(expectedMessages));
    }

    protected void VerifyLastLogMessagesContain(LogLevel minLogLevel, params string[] expectedMessages)
    {
        string[] actualMessages = GetLastLogMessages(minLogLevel, expectedMessages.Length);

        for (int i = 0; i < expectedMessages.Length; i++)
        {
            Assert.That(actualMessages[i], Does.Contain(expectedMessages[i]));
        }
    }

    protected void VerifyLastLogMessagesMatch(LogLevel minLogLevel, params string[] expectedMessagePatterns)
    {
        string[] actualMessages = GetLastLogMessages(minLogLevel, expectedMessagePatterns.Length);

        for (int i = 0; i < expectedMessagePatterns.Length; i++)
        {
            Assert.That(actualMessages[i], Does.Match(expectedMessagePatterns[i]));
        }
    }

    protected void VerifyLastLogEntries(params (LogLevel Level, string Message, Exception Exception)[] expectedLogEntries)
    {
        LogEventInfo[] actualLogEntries = GetLastLogEntries(LogLevel.Trace, expectedLogEntries.Length);

        for (int i = 0; i < expectedLogEntries.Length; i++)
        {
            Assert.That(actualLogEntries[i].Level, Is.EqualTo(expectedLogEntries[i].Level));
            Assert.That(actualLogEntries[i].Message, Is.EqualTo(expectedLogEntries[i].Message));
            Assert.That(actualLogEntries[i].Exception, Is.EqualTo(expectedLogEntries[i].Exception));
        }
    }

    protected LogEventInfo[] GetLastLogEntries(int count) =>
        GetLastLogEntries(LogLevel.Trace, count);

    protected LogEventInfo[] GetLastLogEntries(LogLevel minLogLevel, int count) =>
        LogEntries.Reverse().Where(x => x.Level >= minLogLevel).Take(count).Reverse().ToArray();

    protected string[] GetLastLogMessages(LogLevel minLogLevel, int count) =>
        GetLastLogEntries(minLogLevel, count)
            .Select(x => x.Message)
            .ToArray();
}
