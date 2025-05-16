namespace Atata.NLog.IntegrationTests;

public sealed class NLogFileConsumerTests : TestSuiteBase
{
    [Test]
    public void WithDefaultConfiguration()
    {
        var context = CreateAtataContextWithNLogFileConsumer();

        WriteRandomLogMessageAndAssertItInFile(
            context,
            Path.Combine(context.ArtifactsPath, NLogFileConsumer.DefaultFileName));
    }

    [Test]
    public void WithFileNameTemplate()
    {
        string fileName = Guid.NewGuid().ToString() + ".txt";

        var context = CreateAtataContextWithNLogFileConsumer(
            x => x.WithFileNameTemplate(fileName));

        WriteRandomLogMessageAndAssertItInFile(
            context,
            Path.Combine(AtataContext.ResolveCurrent().ArtifactsPath, fileName));
    }

    [Test]
    public void WithFileNameTemplate_ThatContainsVariables()
    {
        string filePath = "logs/{test-name-sanitized}.log";

        var context = CreateAtataContextWithNLogFileConsumer(
            x => x.WithFileNameTemplate(filePath));

        WriteRandomLogMessageAndAssertItInFile(
            context,
            Path.Combine(
                context.ArtifactsPath,
                "logs",
                $"{context.Test.NameSanitized}.log"));
    }

    [Test]
    public void Log_WithSource() =>
        TestLog(x => x.ForSource("Ext").Trace("Text"), "TRACE {Ext} Text");

    [Test]
    public void Log_WithCategory() =>
        TestLog(x => x.ForCategory("Cat").Trace("Text"), "TRACE [Cat] Text");

    [Test]
    public void Log_WithSourceAndCategory() =>
        TestLog(x => x.ForSource("Ext").ForCategory("Cat").Trace("Text"), "TRACE {Ext} [Cat] Text");

    private static void WriteRandomLogMessageAndAssertItInFile(AtataContext context, string filePath)
    {
        string testMessage = Guid.NewGuid().ToString();

        context.Log.Info(testMessage);

        AssertThatFileShouldContainText(filePath, testMessage);
    }

    private static AtataContext CreateAtataContextWithNLogFileConsumer(
        Action<LogConsumerBuilder<NLogFileConsumer>>? configure = null)
    {
        var contextBuilder = ConfigureSessionlessAtataContext();

        contextBuilder.LogConsumers.AddNLogFile(
            x => configure?.Invoke(x));

        return contextBuilder.Build();
    }

    private static void TestLog(Action<ILogManager> logAction, string expectedText)
    {
        var context = CreateAtataContextWithNLogFileConsumer();

        logAction.Invoke(context.Log);

        AssertThatFileShouldContainText(
            Path.Combine(context.ArtifactsPath, NLogFileConsumer.DefaultFileName),
            expectedText);
    }
}
