namespace Atata.IntegrationTests.Events;

public class DriverDeInitEventTests : UITestFixtureBase
{
    private int _executionsCount;

    private AtataContext _context;

    [SetUp]
    public void SetUp()
    {
        _executionsCount = 0;

        _context = ConfigureAtataContextWithWebDriverSession()
            .EventSubscriptions.Add<DriverDeInitEvent>((eventData, _) =>
            {
                eventData.Driver.Should().NotBeNull().And.Be(_context.Driver);

                _executionsCount++;
            })
            .Build();

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
        _context.RestartDriver();

        _executionsCount.Should().Be(1);
    }
}
