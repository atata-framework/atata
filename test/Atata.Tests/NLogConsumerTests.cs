using System;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests
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
                DefaultAtataContextArtifactsDirectory.BuildPath(),
                $"{AtataContext.Current.TestName}.log");

            AssertThatFileShouldContainText(filePath, testMessage);
        }
    }
}
