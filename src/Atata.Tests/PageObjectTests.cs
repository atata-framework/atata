using System;
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
        public void PageObject_RefreshPageUntil()
        {
            Go.To<WaitingPage>().
                CurrentTime.Get(out TimeSpan? time).
                RefreshPageUntil(x => x.CurrentTime.Value > time.Value.Add(TimeSpan.FromSeconds(15)), timeout: 20, retryInterval: 2);
        }

        [Test]
        public void PageObject_RefreshPageUntil_Timeout()
        {
            var page = Go.To<WaitingPage>().
                CurrentTime.Get(out TimeSpan? time);

            using (StopwatchAsserter.Within(1, 1))
                Assert.Throws<TimeoutException>(() =>
                    page.RefreshPageUntil(x => x.CurrentTime.Value > time.Value.Add(TimeSpan.FromSeconds(15)), timeout: 1));
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

        [Test]
        public void PageObject_ActiveControl()
        {
            Go.To<InputPage>().
                TelInput.Set("123").
                ActiveControl.Attributes.Value.Should.Equal("123").
                IntTextInput.Focus().
                ActiveControl.Attributes.Id.Should.Equal("int-text-input");
        }
    }
}
