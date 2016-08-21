using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class ClickableTest : AutoTest
    {
        [Test]
        public void ControlDelegates()
        {
            Go.To<BasicControlsPage>().
                RawButton.Should().Exist().
                RawButton.Should().BeEnabled().
                RawButton.Content().VerifyEquals("Raw Button").
                RawButton().
                InputButton.Should().Exist().
                InputButton().
                Do(_ => _.LinkButton, x =>
                {
                    x.Should().Exist();
                    x.Content().Should.Equal("Link Button");
                    x();
                }).
                DivButton.Should().Exist().
                DivButton.Click().
                DisabledButton.Should().Exist().
                DisabledButton.Should().BeDisabled().
                MissingButton.Should().Not.Exist();
        }

        [Test]
        public void ControlDelegates_WithNavigaton()
        {
            Go.To<BasicControlsPage>().
                GoToButton.Should().Exist().
                GoToButton.Should().BeEnabled().
                GoToButton();

            Go.To<BasicControlsPage>().
                GoToInputButton.Should().Exist().
                GoToInputButton.Should().BeEnabled().
                GoToInputButton();

            Go.To<BasicControlsPage>().
                GoToLink.Should().Exist().
                GoToLink.Should().BeEnabled().
                GoToLink();

            Go.To<BasicControlsPage>().
                GoToDivButton.Should().Exist().
                GoToDivButton.Should().BeEnabled().
                GoToDivButton();
        }
    }
}
