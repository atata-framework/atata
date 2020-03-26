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
                AddNLogLogging().
                Build();

            string testMessage = Guid.NewGuid().ToString();

            AtataContext.Current.Log.Info(testMessage);

            string filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs",
                AtataContext.BuildStart.Value.ToString(FileScreenshotConsumer.DefaultDateTimeFormat),
                AtataContext.Current.TestName,
                $"{AtataContext.Current.TestName}.log");

            AssertThatFileShouldContainText(filePath, testMessage);
        }
    }
}
