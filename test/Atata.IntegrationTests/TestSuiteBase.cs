namespace Atata.IntegrationTests;

[TestFixture]
public abstract class TestSuiteBase
{
    private FakeLogConsumer _fakeLogConsumer;

    protected FakeLogConsumer CurrentLog =>
        _fakeLogConsumer;

    protected AtataContextBuilder ConfigureSessionlessAtataContext()
    {
        _fakeLogConsumer = new FakeLogConsumer();

        var builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseCulture("en-US")
            .UseNUnitTestName()
            .UseNUnitTestSuiteName()
            .UseNUnitTestSuiteType();

        builder.LogConsumers.AddNUnitTestContext();
        builder.LogConsumers.Add(_fakeLogConsumer);

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
        var actualMessages = CurrentLog.GetMessagesSnapshot(minLogLevel, expectedMessages.Length);

        Assert.That(actualMessages, Is.EqualTo(expectedMessages));
    }

    protected void VerifyLastLogNestingTextsWithMessagesMatch(LogLevel minLogLevel, params string[] expectedMessagePatterns)
    {
        var actualMessages = CurrentLog.GetNestingTextsWithMessagesSnapshot(minLogLevel, expectedMessagePatterns.Length);
        actualMessages.Should().HaveCount(expectedMessagePatterns.Length);

        using (new AssertionScope())
        {
            for (int i = 0; i < expectedMessagePatterns.Length; i++)
            {
                actualMessages[i].Should().MatchRegex(expectedMessagePatterns[i]);
            }
        }
    }

    protected void VerifyLastLogEntries(params (LogLevel Level, string Message, Exception Exception)[] expectedLogEntries)
    {
        var actualLogEntries = CurrentLog.GetSnapshot(LogLevel.Trace, expectedLogEntries.Length);
        actualLogEntries.Should().HaveCount(expectedLogEntries.Length);

        using (new AssertionScope())
        {
            for (int i = 0; i < expectedLogEntries.Length; i++)
            {
                actualLogEntries[i].Level.Should().Be(expectedLogEntries[i].Level);
                actualLogEntries[i].Message.Should().Be(expectedLogEntries[i].Message);
                actualLogEntries[i].Exception.Should().Be(expectedLogEntries[i].Exception);
            }
        }
    }
}
