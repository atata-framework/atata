using NUnit.Framework;

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

            AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
                _page.Result.Should.AtOnce.BePresent());
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
