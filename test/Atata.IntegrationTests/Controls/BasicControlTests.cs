namespace Atata.IntegrationTests.Controls;

public class BasicControlTests : WebDriverSessionTestSuite
{
    [Test]
    public void Interact() =>
        Go.To<BasicControlsPage>()
            .Header.Should.Equal("Basic Controls")

            .ByLabel.FirstName.Should.BePresent()
            .ByLabel.FirstName.Should.BeEnabled()
            .ByLabel.FirstName.Should.Not.AtOnce.BeReadOnly()
            .ByLabel.FirstName.SetRandom(out string firstName)
            .ByLabel.FirstName.Should.Equal(firstName)
            .ById.FirstName.Should.Equal(firstName)

            .ByLabel.LastName.Set("LastName")
            .ByLabel.LastName.Should.Equal("LastName")
            .ById.LastName.Should.Equal("LastName")

            .ById.MiddleName.Set("mdname")
            .Do(_ => _.ByLabel.MiddleName, x =>
                {
                    x.Should.Equal("mdname");
                    x.Set("md2name");
                    x.Should.Not.Equal("mdname");
                })
            .ById.MiddleName.Should.Equal("md2name")

            .ByLabel.ReadonlyField.Should.BePresent()
            .ByLabel.ReadonlyField.Should.BeEnabled()
            .ByLabel.ReadonlyField.Should.BeReadOnly()
            .ByLabel.ReadonlyField.IsReadOnly.Should.BeTrue()
            .ByLabel.ReadonlyField.DomProperties.Get<bool?>("readOnly").Should.BeTrue()
            .ByLabel.ReadonlyField.Should.Equal("readme")

            .ByLabel.DisabledField.Should.BePresent()
            .ByLabel.DisabledField.Should.Not.BeEnabled()
            .ByLabel.DisabledField.IsEnabled.Should.Not.BeTrue()
            .ByLabel.DisabledField.DomProperties.Get<bool?>("disabled").Should.BeTrue()
            .ByLabel.DisabledField.Should.Not.BeReadOnly()
            .ByLabel.DisabledField.Should.Equal("readme")

            .RawButtonControl.Should.BePresent()
            .RawButtonControl.Should.BeEnabled()
            .RawButtonControl.Content.Should.Equal("Raw Button")
            .RawButtonControl.Click()
            .InputButtonControl.Should.BePresent()
            .InputButtonControl.Click()
            .Do(_ => _.LinkButtonControl, x =>
                {
                    x.Should.BePresent();
                    x.Content.Should.Equal("Link Button");
                    x.Content.Should.StartWith("Link");
                    x.Content.Should.EndWith("utton");
                    x.Content.Should.Contain("ink Butto");
                    x.Content.Should.ContainIgnoringCase("k but");
                    x.Click();
                })
            .ClickableControl.Should.BePresent()
            .ClickableControl.Click()
            .DisabledButtonControl.Should.BePresent()
            .DisabledButtonControl.Should.BeDisabled()
            .MissingButtonControl.Should.Not.BePresent();
}
