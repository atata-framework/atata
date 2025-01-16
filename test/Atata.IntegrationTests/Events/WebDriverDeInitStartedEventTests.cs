namespace Atata.IntegrationTests.Events;

public sealed class WebDriverDeInitStartedEventTests : WebDriverSessionTestSuiteBase
{
    private int _executionsCount;

    private AtataContext _context;

    [SetUp]
    public void SetUp()
    {
        _executionsCount = 0;

        var builder = ConfigureAtataContextWithWebDriverSession(session => session
            .EventSubscriptions.Add<WebDriverDeInitStartedEvent>((eventData, _) =>
            {
                eventData.Driver.Should().NotBeNull().And.Be(_context.GetWebDriver());

                _executionsCount++;
            }));

        _context = builder.Build();

        _executionsCount.Should().Be(0);
    }

    [Test]
    public void AfterDispose()
    {
        _context.Dispose();

        _executionsCount.Should().Be(1);
    }

    [Test]
    public void AfterRestartDriver()
    {
        _context.GetWebDriverSession().RestartDriver();

        _executionsCount.Should().Be(1);
    }
}
