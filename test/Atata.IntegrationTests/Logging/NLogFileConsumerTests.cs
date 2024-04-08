namespace Atata.IntegrationTests.Logging;

public class NLogFileConsumerTests : UITestFixtureBase
{
    [Test]
    public void WithDefaultConfiguration()
    {
        ConfigureBaseAtataContext()
            .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
            .LogConsumers.AddNLogFile()
            .Build();

        WriteLogMessageAndAssertItInFile(
            Path.Combine(AtataContext.Current.ArtifactsPath, NLogFileConsumer.DefaultFileName));
    }

    [Test]
    public void WithFileNameTemplate()
    {
        string fileName = Guid.NewGuid().ToString() + ".txt";

        ConfigureBaseAtataContext()
            .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
            .LogConsumers.AddNLogFile()
                .WithFileNameTemplate(fileName)
            .Build();

        WriteLogMessageAndAssertItInFile(
            Path.Combine(AtataContext.Current.ArtifactsPath, fileName));
    }

    [Test]
    public void WithFileNameTemplate_ThatContainsVariables()
    {
        string filePath = "logs/{test-name-sanitized}/{driver-alias}.log";

        ConfigureBaseAtataContext()
            .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
            .LogConsumers.AddNLogFile()
                .WithFileNameTemplate(filePath)
            .Build();

        WriteLogMessageAndAssertItInFile(
            Path.Combine(
                AtataContext.Current.ArtifactsPath,
                "logs",
                AtataContext.Current.Test.NameSanitized,
                $"{WebDriverSession.Current.DriverAlias}.log"));
    }

    private static void WriteLogMessageAndAssertItInFile(string filePath)
    {
        string testMessage = Guid.NewGuid().ToString();

        AtataContext.Current.Log.Info(testMessage);

        AssertThatFileShouldContainText(filePath, testMessage);
    }
}
