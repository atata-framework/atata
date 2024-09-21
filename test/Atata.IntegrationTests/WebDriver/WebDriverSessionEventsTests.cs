namespace Atata.IntegrationTests.WebDriver;

public sealed class WebDriverSessionEventsTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void DriverInitEvent()
    {
        int executionsCount = 0;
        IWebDriver driverOfEvent = null;

        var builder = ConfigureAtataContextWithWebDriverSession(session =>
            session.EventSubscriptions.Add<DriverInitEvent>((eventData, _) =>
            {
                driverOfEvent = eventData.Driver;
                executionsCount++;
            }));
        var context = builder.Build();

        executionsCount.Should().Be(1);
        driverOfEvent.Should().Be(context.GetWebDriver());
    }

    [Test]
    public void DriverInitEvent_WhenRestartDriver()
    {
        int executionsCount = 0;
        IWebDriver initialDriver = null;

        var builder = ConfigureAtataContextWithWebDriverSession(session =>
            session.EventSubscriptions.Add<DriverInitEvent>(eventData =>
            {
                if (executionsCount == 0)
                    initialDriver = eventData.Driver;
                else
                    eventData.Driver.Should().NotBe(initialDriver);

                executionsCount++;
            }));
        var context = builder.Build();

        context.GetWebDriverSession().RestartDriver();

        executionsCount.Should().Be(2);
    }
}
