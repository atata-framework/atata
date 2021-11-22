using System;
using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class AtataContextTimeZoneTests : UITestFixtureBase
    {
        private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC-02");

        private DateTime _nowInSetTimeZone;

        [SetUp]
        public void SetUpTest()
        {
            ConfigureBaseAtataContext()
                .UseTimeZone(_timeZone)
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
                .Build();

            _nowInSetTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
        }

        [Test]
        public void AtataContext_StartedAt() =>
            AssertDateTimeIsCloseToExpected(AtataContext.Current.StartedAt, _nowInSetTimeZone);

        [Test]
        public void AtataContext_BuildStartInTimeZone() =>
            AssertDateTimeIsCloseToExpected(AtataContext.Current.BuildStartInTimeZone, _nowInSetTimeZone, withinMinutes: 20);

        [Test]
        public void LogEventInfo_Timestamp() =>
            AssertDateTimeIsCloseToExpected(LogEntries.Last().Timestamp, _nowInSetTimeZone);

        [Test]
        public void LogEventInfo_TestStart() =>
            AssertDateTimeIsCloseToExpected(LogEntries.Last().TestStart, _nowInSetTimeZone);

        [Test]
        public void LogEventInfo_BuildStart() =>
            AssertDateTimeIsCloseToExpected(LogEntries.Last().BuildStart, _nowInSetTimeZone, withinMinutes: 20);

        private static void AssertDateTimeIsCloseToExpected(DateTime actual, DateTime expected, int withinMinutes = 1) =>
            Assert.That(actual, Is.EqualTo(expected).Within(withinMinutes).Minutes);
    }
}
