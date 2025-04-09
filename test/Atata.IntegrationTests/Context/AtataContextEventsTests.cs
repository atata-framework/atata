namespace Atata.IntegrationTests.Context;

public sealed class AtataContextEventsTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void AtataContextPreInitEvent()
    {
        int executionsCount = 0;

        ConfigureAtataContextWithFakeSession()
            .EventSubscriptions.Add<AtataContextPreInitEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(eventData.Context).And.Be(CurrentContext);

                context.Log.Should().NotBeNull();
                CurrentLog.GetSnapshot().Should().BeEmpty();

                context.Sessions.Should().BeEmpty();

                executionsCount++;
            })
            .Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void AtataContextInitStartedEvent()
    {
        int executionsCount = 0;

        ConfigureAtataContextWithFakeSession()
            .EventSubscriptions.Add<AtataContextInitStartedEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(eventData.Context).And.Be(CurrentContext);

                context.Log.Should().NotBeNull();
                CurrentLog.GetSnapshot().Should().NotBeEmpty();

                context.Sessions.Should().BeEmpty();

                executionsCount++;
            })
            .Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void AtataContextInitCompletedEvent()
    {
        int executionsCount = 0;

        ConfigureAtataContextWithFakeSession()
            .EventSubscriptions.Add<AtataContextInitCompletedEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(eventData.Context).And.Be(CurrentContext);

                context.Log.Should().NotBeNull();
                CurrentLog.GetSnapshot().Should().NotBeEmpty();

                context.Sessions.Should().ContainSingle()
                    .Which.Should().BeOfType<FakeSession>();

                executionsCount++;
            })
            .Build();

        executionsCount.Should().Be(1);
    }
}
