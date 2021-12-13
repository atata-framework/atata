using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class LogManagerTests
    {
        private LogManager _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LogManager(new BasicLogEventInfoFactory());
        }

        [Test]
        public void AddSecretStringsToMask()
        {
            var logConsumer = new EventListLogConsumer();
            _sut.Use(new LogConsumerInfo(logConsumer));

            _sut.AddSecretStringsToMask(
                new[] { new SecretStringToMask("abc123", "***") });

            _sut.Info(@"Set ""abc123"" to something");

            logConsumer.Items[0].Message.Should().Be(@"Set ""***"" to something");
        }
    }
}
