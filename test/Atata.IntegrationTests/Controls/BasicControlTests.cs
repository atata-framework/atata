namespace Atata.IntegrationTests.Controls;

public class BasicControlTests : WebDriverSessionTestSuite
{
    [Test]
    public void Interact() =>
        Go.To<BasicControlsPage>()
            .Header.Should.Be("Basic Controls")

            .ByLabel.FirstName.Should.BePresent()
            .ByLabel.FirstName.Should.BeEnabled()
            .ByLabel.FirstName.Should.Not.AtOnce.BeReadOnly()
            .ByLabel.FirstName.SetRandom(out string firstName)
            .ByLabel.FirstName.Should.Be(firstName)
            .ById.FirstName.Should.Be(firstName)

            .ByLabel.LastName.Set("LastName")
            .ByLabel.LastName.Should.Be("LastName")
            .ById.LastName.Should.Be("LastName")

            .ById.MiddleName.Set("mdname")
            .Do(_ => _.ByLabel.MiddleName, x =>
                {
                    x.Should.Be("mdname");
                    x.Set("md2name");
                    x.Should.Not.Be("mdname");
                })
            .ById.MiddleName.Should.Be("md2name")

            .ByLabel.ReadonlyField.Should.BePresent()
            .ByLabel.ReadonlyField.Should.BeEnabled()
            .ByLabel.ReadonlyField.Should.BeReadOnly()
            .ByLabel.ReadonlyField.IsReadOnly.Should.BeTrue()
            .ByLabel.ReadonlyField.DomProperties.Get<bool?>("readOnly").Should.BeTrue()
            .ByLabel.ReadonlyField.Should.Be("readme")

            .ByLabel.DisabledField.Should.BePresent()
            .ByLabel.DisabledField.Should.Not.BeEnabled()
            .ByLabel.DisabledField.IsEnabled.Should.Not.BeTrue()
            .ByLabel.DisabledField.DomProperties.Get<bool?>("disabled").Should.BeTrue()
            .ByLabel.DisabledField.Should.Not.BeReadOnly()
            .ByLabel.DisabledField.Should.Be("readme")

            .RawButtonControl.Should.BePresent()
            .RawButtonControl.Should.BeEnabled()
            .RawButtonControl.Content.Should.Be("Raw Button")
            .RawButtonControl.Click()
            .InputButtonControl.Should.BePresent()
            .InputButtonControl.Click()
            .Do(_ => _.LinkButtonControl, x =>
                {
                    x.Should.BePresent();
                    x.Content.Should.Be("Link Button");
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
