namespace Atata.IntegrationTests.Controls;

[TestFixture]
public class ControlDelegateTests : UITestFixture
{
    [Test]
    public void WithoutNavigation() =>
        Go.To<BasicControlsPage>()
            .RawButton.Should().Exist()
            .RawButton.Should().BeEnabled()
            .RawButton.Content().Should.Equal("Raw Button")
            .RawButton()
            .InputButton.Should().Exist()
            .InputButton()
            .Do(_ => _.LinkButton, x =>
            {
                x.Should().Exist();
                x.Content().Should.Equal("Link Button");
                x();
            })
            .DivButton.Should().Exist()
            .DivButton.Click()
            .DisabledButton.Should().Exist()
            .DisabledButton.Should().BeDisabled()
            .MissingButton.Should().Not.Exist();

    [Test]
    public void WithNavigaton()
    {
        Go.To<BasicControlsPage>()
            .GoToButton.Should().Exist()
            .GoToButton.Should().BeEnabled()
            .GoToButton();

        Go.To<BasicControlsPage>()
            .GoToInputButton.Should().Exist()
            .GoToInputButton.Should().BeEnabled()
            .GoToInputButton();

        Go.To<BasicControlsPage>()
            .GoToLink.Should().Exist()
            .GoToLink.Should().BeEnabled()
            .GoToLink();

        Go.To<BasicControlsPage>()
            .GoToDivButton.Should().Exist()
            .GoToDivButton.Should().BeEnabled()
            .GoToDivButton();
    }
}
