using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class PageObjectTests : UITestFixture
    {
        [Test]
        public void PageObject_RefreshPage()
        {
            Go.To<BasicControlsPage>().
                ById.FirstName.Set("test").
                ById.FirstName.Should.Equal("test").
                RefreshPage().
                ById.FirstName.Should.BeNull();
        }

        [Test]
        public void PageObject_IgnoreInit()
        {
            Assert.That(Go.To<BasicControlsPage>().IgnoredInput, Is.Null);
        }

        [Test]
        public void PageObject_GoBack()
        {
            Go.To<GoTo1Page>().
                GoTo2().
                    GoBack<GoTo1Page>().
                    Should.Exist();
        }

        [Test]
        public void PageObject_GoBack_Fail()
        {
            var page2 = Go.To<GoTo1Page>().
                GoTo2();

            Assert.Throws<AssertionException>(
                () => page2.GoBack<GoTo3Page>());
        }

        [Test]
        public void PageObject_GoForward()
        {
            Go.To<GoTo1Page>().
                GoTo2().
                    GoBack<GoTo1Page>().
                GoForward<GoTo2Page>().
                    Should.Exist();
        }

        [Test]
        public void PageObject_GoForward_Failed()
        {
            var page1 = Go.To<GoTo1Page>().
                GoTo2().
                    GoBack<GoTo1Page>();

            Assert.Throws<AssertionException>(
                () => page1.GoForward<GoTo3Page>());
        }

        [Test]
        public void PageObject_Press()
        {
            Go.To<InputPage>().
                TextInput.Focus().
                Press("abc").
                Press("d").
                Press(Keys.Tab).
                Press("e").
                TextInput.Should.Equal("abcd");
        }
    }
}
