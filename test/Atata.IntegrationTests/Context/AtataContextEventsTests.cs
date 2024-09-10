using OpenQA.Selenium.Chrome;

namespace Atata.IntegrationTests.Context;

public class AtataContextEventsTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void Init()
    {
        int executionsCount = 0;

        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.EventSubscriptions.Add<AtataContextInitStartedEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

                context.Log.Should().NotBeNull();
                context.GetWebDriverSession().Driver.Should().BeNull();

                executionsCount++;
            });
        builder.Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void Init_WithNullDriver()
    {
        int executionsCount = 0;
        var builder = ConfigureSessionlessAtataContext();
        builder.Sessions.AddWebDriver(x => x.UseDriver(() => null));
        builder.EventSubscriptions.Add<AtataContextInitStartedEvent>(() => executionsCount++);

        Assert.Throws<WebDriverInitializationException>(
            () => builder.Build());

        executionsCount.Should().Be(1);
    }

    [Test]
    public void InitCompleted()
    {
        int executionsCount = 0;

        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.EventSubscriptions.Add<AtataContextInitCompletedEvent>((eventData, context) =>
        {
            eventData.Should().NotBeNull();
            context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

            context.Log.Should().NotBeNull();
            context.GetWebDriver().Should().BeOfType<ChromeDriver>();

            executionsCount++;
        });
        builder.Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void DriverInit()
    {
        int executionsCount = 0;

        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.EventSubscriptions.Add<DriverInitEvent>((eventData, context) =>
        {
            eventData.Should().NotBeNull();
            context.Should().NotBeNull().And.Be(AtataContext.Current);

            context.Log.Should().NotBeNull();
            eventData.Driver.Should().NotBeNull().And.Be(AtataContext.Current.GetWebDriver());

            executionsCount++;
        });
        builder.Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void DriverInit_WhenRestartDriver()
    {
        int executionsCount = 0;
        IWebDriver initialDriver = null;

        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.EventSubscriptions.Add<DriverInitEvent>(eventData =>
        {
            if (executionsCount == 0)
                initialDriver = eventData.Driver;
            else
                eventData.Driver.Should().NotBe(initialDriver);

            executionsCount++;
        });
        var context = builder.Build();

        context.RestartDriver();

        executionsCount.Should().Be(2);
    }
}
