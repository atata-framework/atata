namespace Atata.IntegrationTests.Sessions;

public sealed class AtataSessionEventsTests
{
    [Test]
    public async Task OrderOfExecution()
    {
        FakeLogConsumer fakeLogConsumer = new();

        var parentContextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
            .Sessions.Add<FakeSessionBuilder>(session =>
            {
                session.EventSubscriptions.Add<AtataSessionInitStartedEvent>(
                    x => x.Session.Log.Info(nameof(AtataSessionInitStartedEvent)));

                session.EventSubscriptions.Add<AtataSessionInitCompletedEvent>(
                    x => x.Session.Log.Info(nameof(AtataSessionInitCompletedEvent)));

                session.EventSubscriptions.Add<AtataSessionAssignedToContextEvent>(
                    x => x.Session.Log.Info(nameof(AtataSessionAssignedToContextEvent)));

                session.EventSubscriptions.Add<AtataSessionUnassignedFromContextEvent>(
                    x => x.Session.Log.Info(nameof(AtataSessionUnassignedFromContextEvent)));

                session.EventSubscriptions.Add<AtataSessionDeInitStartedEvent>(
                    x => x.Session.Log.Info(nameof(AtataSessionDeInitStartedEvent)));

                session.EventSubscriptions.Add<AtataSessionDeInitCompletedEvent>(
                    x => x.Session.Log.Info(nameof(AtataSessionDeInitCompletedEvent)));

                session.Mode = AtataSessionMode.Shared;
            });
        parentContextBuilder.LogConsumers.Add(fakeLogConsumer)
            .WithEmbedSessionLog(false);

        await using (var parentContext = await parentContextBuilder.BuildAsync())
        {
            var childContextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.Borrow<FakeSession>();
            childContextBuilder.LogConsumers.Add(fakeLogConsumer)
                .WithEmbedSessionLog(false);

            var childContext = await childContextBuilder.BuildAsync();
            await childContext.DisposeAsync();
        }

        var sessionLogMessages = fakeLogConsumer.GetSnapshot()
            .Where(x => x.Session is not null)
            .Select(x => x.NestingText + x.Message)
            .ToArray();

        string wholeSessionLog = string.Join(Environment.NewLine, sessionLogMessages);

        wholeSessionLog.Should().Match(
            """
            > Initialize FakeSession { * }
            - AtataSessionInitStartedEvent
            - AtataSessionInitCompletedEvent
            - AtataSessionAssignedToContextEvent
            < Initialize FakeSession { * } (*)
            FakeSession { * } is borrowed by AtataContext { * }
            AtataSessionUnassignedFromContextEvent
            AtataSessionAssignedToContextEvent
            FakeSession { * } is borrowed from AtataContext { * }
            FakeSession { * } is returned to AtataContext { * }
            AtataSessionUnassignedFromContextEvent
            AtataSessionAssignedToContextEvent
            FakeSession { * } is returned by AtataContext { * }
            > Deinitialize FakeSession { * }
            - AtataSessionDeInitStartedEvent
            - AtataSessionDeInitCompletedEvent
            < Deinitialize FakeSession { * } (*)
            """);
    }
}
