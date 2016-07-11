using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class ClickableTest : TestBase
    {
        [Test]
        public void ControlDelegates()
        {
            Go.To<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                RawButton.Content().VerifyEquals("Raw Button").
                RawButton().
                InputButton.VerifyExists().
                InputButton().
                Do(_ => _.LinkButton, x =>
                {
                    x.VerifyExists();
                    x.Content().Should.Equal("Link Button");
                    x();
                }).
                DivButton.VerifyExists().
                DivButton.Click().
                DisabledButton.VerifyExists().
                DisabledButton.VerifyDisabled().
                MissingButton.VerifyMissing();
        }

        [Test]
        public void ControlDelegates_WithNavigaton()
        {
            Go.To<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToButton();

            Go.To<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToInputButton();

            Go.To<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToLink();

            Go.To<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToDivButton();
        }
    }
}
