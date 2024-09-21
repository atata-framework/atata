namespace Atata.IntegrationTests.Logging;

public class NLogConsumerTests : TestSuiteBase
{
    [Test]
    public void WithDefaultConfiguration()
    {
        // Arrange
        var builder = ConfigureSessionlessAtataContext();
        builder.LogConsumers.AddNLog();
        var context = builder.Build();

        // Act
        string testMessage = Guid.NewGuid().ToString();
        context.Log.Info(testMessage);

        // Assert
        string filePath = Path.Combine(context.ArtifactsPath, $"{context.Test.NameSanitized}.log");

        AssertThatFileShouldContainText(filePath, testMessage);
    }
}
