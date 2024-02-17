namespace Atata.IntegrationTests.Context;

[Parallelizable(ParallelScope.None)]
public class AtataContextTimeZoneTests : UITestFixtureBase
{
    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC-02");

    private DateTime _nowInSetTimeZone;

    [OneTimeSetUp]
    public void SetUpFixture() =>
        AtataContext.GlobalProperties.UseTimeZone(_timeZone);

    [OneTimeTearDown]
    public void TearDownFixture() =>
        AtataContext.GlobalProperties.UseTimeZone(TimeZoneInfo.Local);

    [SetUp]
    public void SetUp()
    {
        ConfigureBaseAtataContext()
            .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
            .Build();

        _nowInSetTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
    }

    [Test]
    public void AtataContext_StartedAt() =>
        AssertDateTimeIsCloseToExpected(AtataContext.Current.StartedAt, _nowInSetTimeZone);

    [Test]
    public void AtataContext_GlobalProperties_BuildStart() =>
        AssertDateTimeIsCloseToExpected(AtataContext.GlobalProperties.BuildStart, _nowInSetTimeZone, withinMinutes: 30);

    [Test]
    public void LogEventInfo_Timestamp() =>
        AssertDateTimeIsCloseToExpected(LogEntries.Last().Timestamp, _nowInSetTimeZone);

    private static void AssertDateTimeIsCloseToExpected(DateTime actual, DateTime expected, int withinMinutes = 1) =>
        Assert.That(actual, Is.EqualTo(expected).Within(withinMinutes).Minutes);
}
