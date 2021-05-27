using System;
using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class AtataContextTimeZoneTests : UITestFixtureBase
    {
        private readonly TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC-02");

        private DateTime nowInSetTimeZone;

        [SetUp]
        public void SetUpTest()
        {
            ConfigureBaseAtataContext()
                .UseTimeZone(timeZone)
                .Build();

            nowInSetTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        }

        [Test]
        public void AtataContext_StartedAt()
        {
            AssertDateTimeIsCloseToExpected(AtataContext.Current.StartedAt, nowInSetTimeZone);
        }

        [Test]
        public void AtataContext_BuildStartInTimeZone()
        {
            AssertDateTimeIsCloseToExpected(AtataContext.Current.BuildStartInTimeZone, nowInSetTimeZone);
        }

        [Test]
        public void LogEventInfo_Timestamp()
        {
            AssertDateTimeIsCloseToExpected(LogEntries.Last().Timestamp, nowInSetTimeZone);
        }

        [Test]
        public void LogEventInfo_TestStart()
        {
            AssertDateTimeIsCloseToExpected(LogEntries.Last().TestStart, nowInSetTimeZone);
        }

        [Test]
        public void LogEventInfo_BuildStart()
        {
            AssertDateTimeIsCloseToExpected(LogEntries.Last().BuildStart, nowInSetTimeZone);
        }

        private static void AssertDateTimeIsCloseToExpected(DateTime actual, DateTime expected)
        {
            Assert.That(actual, Is.EqualTo(expected).Within(2).Minutes);
        }
    }
}
