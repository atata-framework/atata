using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class GoToTest : TestBase
    {
        [Test]
        public void GoTo_SimpleNavigation()
        {
            Go.To<GoTo1Page>();
            Go.To<GoTo2Page>();
        }

        [Test]
        public void GoTo_LinkNavigation()
        {
            var page1 = Go.To<GoTo1Page>();
            var page1Returned = page1.
                GoTo2().
                    GoTo1();

            Assert.That(page1Returned, Is.Not.EqualTo(page1));
        }

        [Test]
        public void GoTo_UrlNavigation()
        {
            Go.To<GoTo1Page>();
            string url = "http://localhost:50549/GoTo2.html?somearg=1";
            Go.To<GoTo2Page>(url: url);

            Assert.That(AtataContext.Current.Driver.Url, Is.EqualTo(url));
        }

        [Test]
        public void GoTo_Temporarily()
        {
            var page1 = Go.To<GoTo1Page>();
            Go.To<GoTo2Page>(temporarily: true);
            var page1Returned = Go.To<GoTo1Page>();

            Assert.That(page1Returned, Is.EqualTo(page1));
        }

        [Test]
        public void GoTo_TemporarilyByLink()
        {
            var page1 = Go.To<GoTo1Page>();
            var page1Returned = page1.
                GoTo2Temporarily().
                    GoTo1();

            Assert.That(page1Returned, Is.EqualTo(page1));
        }

        [Test]
        public void GoTo_TemporarilyByLink2()
        {
            var page1 = Go.To<GoTo1Page>();
            var page1Returned = page1.
                GoTo2Temporarily().
                    GoTo1Temporarily();

            Assert.That(page1Returned, Is.EqualTo(page1));
        }
    }
}
