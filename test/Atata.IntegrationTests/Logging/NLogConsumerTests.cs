namespace Atata.IntegrationTests.Logging;

public class NLogConsumerTests : UITestFixtureBase
{
    [Test]
    public void WithDefaultConfiguration()
    {
        ConfigureBaseAtataContext()
            .LogConsumers.AddNLog()
            .Build();

        string testMessage = Guid.NewGuid().ToString();

        AtataContext.Current.Log.Info(testMessage);

        string filePath = Path.Combine(
            AtataContext.Current.Artifacts.FullName,
            $"{AtataContext.Current.Test.NameSanitized}.log");

        AssertThatFileShouldContainText(filePath, testMessage);
    }
}
