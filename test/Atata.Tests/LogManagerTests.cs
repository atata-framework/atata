using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class LogManagerTests
    {
        private LogManager sut;

        [SetUp]
        public void SetUp()
        {
            sut = new LogManager();
        }

        [Test]
        public void AddSecretStringsToMask()
        {
            var logConsumer = new EventListLogConsumer();
            sut.Use(new LogConsumerInfo(logConsumer));

            sut.AddSecretStringsToMask(
                new[] { new SecretStringToMask("abc123", "***") });

            sut.Info(@"Set ""abc123"" to something");

            logConsumer.Items[0].Message.Should().Be(@"Set ""***"" to something");
        }
    }
}
