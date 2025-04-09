namespace Atata.IntegrationTests.Context;

public static class AtataContextGlobalPropertiesTests
{
    [Parallelizable(ParallelScope.None)]
    public class UseArtifactsRootPathTemplate : SessionlessTestSuite
    {
        private readonly string _rootPath = $"{AppDomain.CurrentDomain.BaseDirectory}artifacts{Path.DirectorySeparatorChar}{Guid.NewGuid()}";

        [OneTimeSetUp]
        public void SetUpFixture() =>
            AtataContext.GlobalProperties.UseArtifactsRootPathTemplate(_rootPath);

        [OneTimeTearDown]
        public void TearDownFixture() =>
            AtataContext.GlobalProperties.UseArtifactsRootPathTemplate(AtataContextGlobalProperties.DefaultArtifactsRootPathTemplate);

        [Test]
        public void AtataContext_Artifacts() =>
            CurrentContext.Artifacts.FullName.Should.StartWith(_rootPath + Path.DirectorySeparatorChar);
    }

    [Parallelizable(ParallelScope.None)]
    public class UseTimeZone : SessionlessTestSuite
    {
        private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC-02");

        private DateTime _nowInSetTimeZone;

        [OneTimeSetUp]
        public void SetUpFixture() =>
            AtataContext.GlobalProperties.UseTimeZone(_timeZone);

        [OneTimeTearDown]
        public void TearDownFixture() =>
            AtataContext.GlobalProperties.UseTimeZone(TimeZoneInfo.Local);

        protected override void OnSetUp() =>
            _nowInSetTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);

        [Test]
        public void AtataContext_StartedAt() =>
            AssertDateTimeIsCloseToExpected(CurrentContext.StartedAt, _nowInSetTimeZone);

        [Test]
        public void AtataContext_GlobalProperties_RunStart() =>
            AssertDateTimeIsCloseToExpected(AtataContext.GlobalProperties.RunStart, _nowInSetTimeZone, withinMinutes: 30);

        [Test]
        public void LogEventInfo_Timestamp() =>
            AssertDateTimeIsCloseToExpected(CurrentLog.LatestRecord.Timestamp, _nowInSetTimeZone);

        private static void AssertDateTimeIsCloseToExpected(DateTime actual, DateTime expected, int withinMinutes = 1) =>
            Assert.That(actual, Is.EqualTo(expected).Within(withinMinutes).Minutes);
    }
}
