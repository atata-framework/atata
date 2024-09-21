namespace Atata.IntegrationTests.Context;

public class AtataContextEventsTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void AtataContextPreInitEvent()
    {
        int executionsCount = 0;

        var builder = ConfigureAtataContextWithFakeSession();
        builder.EventSubscriptions.Add<AtataContextPreInitEvent>((eventData, context) =>
        {
            eventData.Should().NotBeNull();
            context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

            context.Log.Should().NotBeNull();
            CurrentLog.GetSnapshot().Should().BeEmpty();

            context.Sessions.Should().BeEmpty();

            executionsCount++;
        });
        builder.Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void AtataContextInitStartedEvent()
    {
        int executionsCount = 0;

        var builder = ConfigureAtataContextWithFakeSession();
        builder.EventSubscriptions.Add<AtataContextInitStartedEvent>((eventData, context) =>
            {
                eventData.Should().NotBeNull();
                context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

                context.Log.Should().NotBeNull();
                CurrentLog.GetSnapshot().Should().NotBeEmpty();

                context.Sessions.Should().BeEmpty();

                executionsCount++;
            });
        builder.Build();

        executionsCount.Should().Be(1);
    }

    [Test]
    public void AtataContextInitCompletedEvent()
    {
        int executionsCount = 0;

        var builder = ConfigureAtataContextWithFakeSession();
        builder.EventSubscriptions.Add<AtataContextInitCompletedEvent>((eventData, context) =>
        {
            eventData.Should().NotBeNull();
            context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

            context.Log.Should().NotBeNull();
            CurrentLog.GetSnapshot().Should().NotBeEmpty();

            context.Sessions.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSession>();

            executionsCount++;
        });
        builder.Build();

        executionsCount.Should().Be(1);
    }
}
