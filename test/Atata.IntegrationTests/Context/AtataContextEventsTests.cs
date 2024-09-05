using OpenQA.Selenium.Chrome;

namespace Atata.IntegrationTests.Context;

public class AtataContextEventsTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void Init()
    {
        int executionsCount = 0;

        ConfigureAtataContextWithWebDriverSession()
            .EventSubscriptions.Add<AtataContextInitStartedEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

                context.Log.Should().NotBeNull();
                context.GetWebDriverSession().Driver.Should().BeNull();

                executionsCount++;
            })
            .Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void Init_WithNullDriver()
    {
        int executionsCount = 0;

        Assert.Throws<WebDriverInitializationException>(() =>
            AtataContext.Configure()
                .UseDriver(() => null)
                .EventSubscriptions.Add<AtataContextInitStartedEvent>(() => executionsCount++)
                .Build());

        executionsCount.Should().Be(1);
    }

    [Test]
    public void InitCompleted()
    {
        int executionsCount = 0;

        ConfigureAtataContextWithWebDriverSession()
            .EventSubscriptions.Add<AtataContextInitCompletedEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

                context.Log.Should().NotBeNull();
                context.Driver.Should().BeOfType<ChromeDriver>();

                executionsCount++;
            })
            .Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void DriverInit()
    {
        int executionsCount = 0;

        ConfigureAtataContextWithWebDriverSession()
            .EventSubscriptions.Add<DriverInitEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(AtataContext.Current);

                context.Log.Should().NotBeNull();
                eventData.Driver.Should().NotBeNull().And.Be(AtataContext.Current.Driver);

                executionsCount++;
            })
            .Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void DriverInit_WhenRestartDriver()
    {
        int executionsCount = 0;
        IWebDriver initialDriver = null;

        ConfigureAtataContextWithWebDriverSession()
            .EventSubscriptions.Add<DriverInitEvent>(eventData =>
            {
                if (executionsCount == 0)
                    initialDriver = eventData.Driver;
                else
                    eventData.Driver.Should().NotBe(initialDriver);

                executionsCount++;
            })
            .Build();

        AtataContext.Current.RestartDriver();

        executionsCount.Should().Be(2);
    }
}
