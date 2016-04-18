using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class GoToTest : TestBase
    {
        [Test]
        public void GoTo_SimpleNavigation()
        {
            var page1 = Go.To<GoTo1Page>();
            AssertCurrentPageObject(page1);

            var page2 = Go.To<GoTo2Page>();
            AssertCurrentPageObject(page2);
        }

        [Test]
        public void GoTo_LinkNavigation()
        {
            var page1 = Go.To<GoTo1Page>();

            AssertNoTemporarilyPreservedPageObjects();

            var page1Returned = page1.
                GoTo2().
                    GoTo1();

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(page1Returned, Is.Not.EqualTo(page1));
        }

        [Test]
        public void GoTo_UrlNavigation()
        {
            Go.To<GoTo1Page>();
            string url = "http://localhost:50549/GoTo2.html?somearg=1";
            Go.To<GoTo2Page>(url: url);

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(AtataContext.Current.Driver.Url, Is.EqualTo(url));
        }

        [Test]
        public void GoTo_Temporarily()
        {
            var page1 = Go.To<GoTo1Page>();
            Go.To<GoTo2Page>(temporarily: true);

            AssertTemporarilyPreservedPageObjects(page1);

            var page1Returned = Go.To<GoTo1Page>();

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(page1Returned, Is.EqualTo(page1));
        }

        [Test]
        public void GoTo_TemporarilyByLink()
        {
            var page1 = Go.To<GoTo1Page>();

            var page2 = page1.
                GoTo2Temporarily();

            AssertTemporarilyPreservedPageObjects(page1);

            var page1Returned = page2.
                GoTo1();

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(page1Returned, Is.EqualTo(page1));
        }

        [Test]
        public void GoTo_TemporarilyByLink2()
        {
            var page1 = Go.To<GoTo1Page>();

            var page2 = page1.
                GoTo2Temporarily();

            AssertTemporarilyPreservedPageObjects(page1);

            var page1Returned = page2.
                GoTo1Temporarily();

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(page1Returned, Is.EqualTo(page1));
        }

        private void AssertCurrentPageObject(UIComponent pageObject)
        {
            Assert.That(AtataContext.Current.PageObject, Is.EqualTo(pageObject));
        }

        private void AssertNoTemporarilyPreservedPageObjects()
        {
            Assert.That(AtataContext.Current.TemporarilyPreservedPageObjects, Is.Empty);
        }

        private void AssertTemporarilyPreservedPageObjects(params UIComponent[] pageObjects)
        {
            Assert.That(AtataContext.Current.TemporarilyPreservedPageObjects.Count, Is.EqualTo(pageObjects.Length));
            Assert.That(AtataContext.Current.TemporarilyPreservedPageObjects, Is.EquivalentTo(pageObjects));
        }
    }
}
