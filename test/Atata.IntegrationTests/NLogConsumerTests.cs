using System;
using System.IO;
using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class NLogConsumerTests : UITestFixtureBase
    {
        [Test]
        public void NLogConsumer()
        {
            ConfigureBaseAtataContext().
                LogConsumers.AddNLog().
                Build();

            string testMessage = Guid.NewGuid().ToString();

            AtataContext.Current.Log.Info(testMessage);

            string filePath = Path.Combine(
                AtataContext.Current.Artifacts.FullName,
                $"{AtataContext.Current.TestNameSanitized}.log");

            AssertThatFileShouldContainText(filePath, testMessage);
        }
    }
}
