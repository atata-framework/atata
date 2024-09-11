namespace Atata.IntegrationTests.Logging;

public class NLogFileConsumerTests : SessionlessTestSuite
{
    [Test]
    public void WithDefaultConfiguration()
    {
        var context = CreateAtataContextWithNLogFileConsumer();

        WriteLogMessageAndAssertItInFile(
            context,
            Path.Combine(context.ArtifactsPath, NLogFileConsumer.DefaultFileName));
    }

    [Test]
    public void WithFileNameTemplate()
    {
        string fileName = Guid.NewGuid().ToString() + ".txt";

        var context = CreateAtataContextWithNLogFileConsumer(
            x => x.WithFileNameTemplate(fileName));

        WriteLogMessageAndAssertItInFile(
            context,
            Path.Combine(AtataContext.Current.ArtifactsPath, fileName));
    }

    [Test]
    public void WithFileNameTemplate_ThatContainsVariables()
    {
        string filePath = "logs/{test-name-sanitized}.log";

        var context = CreateAtataContextWithNLogFileConsumer(
            x => x.WithFileNameTemplate(filePath));

        WriteLogMessageAndAssertItInFile(
            context,
            Path.Combine(
                context.ArtifactsPath,
                "logs",
                $"{context.Test.NameSanitized}.log"));
    }

    private static void WriteLogMessageAndAssertItInFile(AtataContext context, string filePath)
    {
        string testMessage = Guid.NewGuid().ToString();

        context.Log.Info(testMessage);

        AssertThatFileShouldContainText(filePath, testMessage);
    }

    private AtataContext CreateAtataContextWithNLogFileConsumer(
        Action<LogConsumerBuilder<NLogFileConsumer>> configure = null)
    {
        var contextBuilder = ConfigureSessionlessAtataContext();

        var logConsumerBuilder = contextBuilder.LogConsumers.AddNLogFile();
        configure?.Invoke(logConsumerBuilder);

        return contextBuilder.Build();
    }
}
