using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class GoTest : TestBase
    {
        [Test]
        public void Go_To_SimpleNavigation()
        {
            var page1 = Go.To<GoTo1Page>();
            AssertCurrentPageObject(page1);

            var page2 = Go.To<GoTo2Page>();
            AssertCurrentPageObject(page2);
        }

        [Test]
        public void Go_To_LinkNavigation()
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
        public void Go_To_AbsoluteUrlNavigation()
        {
            Go.To<GoTo1Page>();
            string url = "http://localhost:50549/GoTo2.html?somearg=1";
            Go.To<GoTo2Page>(url: url);

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(ATContext.Current.Driver.Url, Is.EqualTo(url));
        }

        [Test]
        public void Go_To_RelativeUrlNavigation()
        {
            Go.To<GoTo1Page>();
            string url = "GoTo2.html?somearg=1";
            Go.To<GoTo2Page>(url: url);

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(ATContext.Current.Driver.Url, Does.EndWith(url));
        }

        [Test]
        public void Go_To_Temporarily()
        {
            var page1 = Go.To<GoTo1Page>();
            Go.To<GoTo2Page>(temporarily: true);

            AssertTemporarilyPreservedPageObjects(page1);

            var page1Returned = Go.To<GoTo1Page>();

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(page1Returned, Is.EqualTo(page1));
        }

        [Test]
        public void Go_To_TemporarilyByLink()
        {
            var page1 = Go.To<GoTo1Page>();

            var page2 = page1.
                GoTo2Temporarily();

            AssertTemporarilyPreservedPageObjects(page1);

            var page1Returned = page2.
                GoTo1();

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(page1Returned, Is.EqualTo(page1));

            page1Returned.
                GoTo2();
        }

        [Test]
        public void Go_To_TemporarilyByLink2()
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

        [Test]
        public void Go_To_TemporarilyByLinkComplex()
        {
            var page1 = Go.To<GoTo1Page>();

            var page2 = page1.
                GoTo2Temporarily();

            AssertTemporarilyPreservedPageObjects(page1);

            var page3 = page2.
                GoTo3Temporarily();

            AssertTemporarilyPreservedPageObjects(page1, page2);

            var page1Returned = page3.
                GoTo1();

            AssertNoTemporarilyPreservedPageObjects();
            Assert.That(page1Returned, Is.EqualTo(page1));
            AssertCurrentPageObject(page1);
        }

        [Test]
        public void Go_ToNextWindow()
        {
            var page1 = Go.To<GoTo1Page>();

            page1.
                GoTo2Blank();

            Go.ToNextWindow<GoTo2Page>().
                CloseWindow();

            AssertNoTemporarilyPreservedPageObjects();

            Go.To<GoTo1Page>(navigate: false);
        }

        [Test]
        public void Go_ToNextWindow_Temporarily()
        {
            var page1 = Go.To<GoTo1Page>().
                GoTo2Blank();

            Go.ToNextWindow<GoTo2Page>(temporarily: true).
                CloseWindow();

            AssertTemporarilyPreservedPageObjects(page1);

            Go.To<GoTo1Page>(navigate: false);

            AssertCurrentPageObject(page1);
        }

        [Test]
        public void Go_ToNextWindow_TemporarilyComplex()
        {
            var page1 = Go.To<GoTo1Page>().
                GoTo2Blank();

            var page2 = Go.ToNextWindow<GoTo2Page>(temporarily: true);

            page2.
                GoTo3Temporarily().
                CloseWindow();

            AssertTemporarilyPreservedPageObjects(page1, page2);

            var page1Returned = Go.To<GoTo1Page>(navigate: false);

            Assert.That(page1Returned, Is.EqualTo(page1));
            AssertCurrentPageObject(page1);
        }

        private void AssertCurrentPageObject(UIComponent pageObject)
        {
            Assert.That(ATContext.Current.PageObject, Is.EqualTo(pageObject));
        }

        private void AssertNoTemporarilyPreservedPageObjects()
        {
            Assert.That(ATContext.Current.TemporarilyPreservedPageObjects, Is.Empty);
        }

        private void AssertTemporarilyPreservedPageObjects(params UIComponent[] pageObjects)
        {
            Assert.That(ATContext.Current.TemporarilyPreservedPageObjects.Count, Is.EqualTo(pageObjects.Length));
            Assert.That(ATContext.Current.TemporarilyPreservedPageObjects, Is.EquivalentTo(pageObjects));
        }
    }
}
