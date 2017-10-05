using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class WaitingTests : UITestFixture
    {
        private WaitingPage page;

        protected override void OnSetUp()
        {
            page = Go.To<WaitingPage>();
        }

        [Test]
        public void Waiting_BuiltIn_Disabled()
        {
            page.
                ButtonWithoutWait.Click();

            Assert.Throws<NoSuchElementException>(
                () => page.Result.Should.AtOnce.Exist());
        }

        [Test]
        public void Waiting_BuiltIn()
        {
            page.
                ButtonWithoutWait.Click().
                Result.Should.Exist();
        }
    }
}
