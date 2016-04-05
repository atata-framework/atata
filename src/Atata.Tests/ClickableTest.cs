using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class ClickableTest : TestBase
    {
        [Test]
        public void ControlDelegates()
        {
            GoTo<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                RawButton.VerifyContent("Raw Button").
                RawButton().
                InputButton.VerifyExists().
                InputButton().
                Do(_ => _.LinkButton, x =>
                {
                    x.VerifyExists();
                    x.VerifyContent("Link Button");
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
            GoTo<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToButton();

            GoTo<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToInputButton();

            GoTo<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToLink();

            GoTo<BasicControlsPage>().
                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                GoToDivButton();
        }
    }
}
