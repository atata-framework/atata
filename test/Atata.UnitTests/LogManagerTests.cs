namespace Atata.UnitTests;

[TestFixture]
public class LogManagerTests
{
    private LogManager _sut;

    [SetUp]
    public void SetUp() =>
        _sut = new LogManager(new BasicLogEventInfoFactory());

    [Test]
    public void AddSecretStringsToMask()
    {
        var logConsumerMock = new Mock<ILogConsumer>();
        _sut.Use(new LogConsumerConfiguration(logConsumerMock.Object));

        _sut.AddSecretStringsToMask(
            [new SecretStringToMask("abc123", "***")]);

        _sut.Info(@"Set ""abc123"" to something");

        logConsumerMock.Verify(x => x.Log(It.Is<LogEventInfo>(eventInfo => eventInfo.Message == @"Set ""***"" to something")), Times.Once);
    }
}
