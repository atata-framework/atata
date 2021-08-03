using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class WaitingTests : UITestFixture
    {
        private WaitingPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<WaitingPage>();
        }

        [Test]
        public void Waiting_BuiltIn_Disabled()
        {
            _page.
                ButtonWithoutWait.Click();

            AssertThrowsWithInnerException<AssertionException, NoSuchElementException>(
                () => _page.Result.Should.AtOnce.Exist());
        }

        [Test]
        public void Waiting_BuiltIn()
        {
            _page.
                ButtonWithoutWait.Click().
                Result.Should.Exist();
        }
    }
}
