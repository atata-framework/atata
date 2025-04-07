namespace Atata.IntegrationTests.Logging;

public sealed class HybridLogTests : TestSuiteBase
{
    [Test]
    public async Task SessionsInParallel()
    {
        var builder = ConfigureSessionlessAtataContext();
        builder.Sessions.Add<FakeSessionBuilder>();
        builder.Sessions.Add<FakeSessionBuilder>();
        var context = builder.Build();

        await context.Log.ExecuteSectionAsync(new LogSection("context section"), async () =>
            await Task.WhenAll(
                Task.Run(() =>
                {
                    var session = context.Sessions.Get<FakeSession>(0);
                    session.Log.Trace("s1 trace");
                    session.Log.ExecuteSection("s1 section", () =>
                    {
                        session.Log.Trace("s1 sub-trace 1");
                        Thread.Sleep(50);
                        session.Log.Trace("s1 sub-trace 2");
                    });
                }),
                Task.Run(() =>
                {
                    var session = context.Sessions.Get<FakeSession>(1);
                    session.Log.Trace("s2 trace");
                    session.Log.ExecuteSection("s2 section", () =>
                    {
                        session.Log.Trace("s2 sub-trace 1");
                        Thread.Sleep(50);
                        session.Log.Trace("s2 sub-trace 2");
                    });
                })));

        var logMessages = CurrentLog.GetNestingTextsWithMessagesSnapshot(12)
            .ToSubject("logNestingTextsWithMessages");
        logMessages.ElementAt(0).Should.Be("> context section");
        logMessages.Skip(1).Take(10).Should.ConsistOf(
            x => x == "- s1 trace",
            x => x == "- s2 trace",
            x => x == "- > s1 section",
            x => x == "- > s2 section",
            x => x == "- - s1 sub-trace 1",
            x => x == "- - s1 sub-trace 2",
            x => x == "- - s2 sub-trace 1",
            x => x == "- - s2 sub-trace 2",
            x => x!.StartsWith("- < s1 section"),
            x => x!.StartsWith("- < s2 section"));
        logMessages.ElementAt(11).Should.StartWith("< context section");
    }
}
