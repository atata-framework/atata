using NUnit.Framework;

namespace Atata.Tests
{
    public class BasicControlTests : UITestFixture
    {
        [Test]
        public void BasicControlInteraction()
        {
            Go.To<BasicControlsPage>().
                Header.Should.Equal("Basic Controls").

                ByLabel.FirstName.Should.Exist().
                ByLabel.FirstName.Should.BeEnabled().
                ByLabel.FirstName.Should.Not.AtOnce.BeReadOnly().
                ByLabel.FirstName.SetRandom(out string firstName).
                ByLabel.FirstName.Should.Equal(firstName).
                ById.FirstName.Should.Equal(firstName).

                ByLabel.LastName.Set("LastName").
                ByLabel.LastName.Should.Equal("LastName").
                ById.LastName.Should.Equal("LastName").

                ById.MiddleName.Set("mdname").
                Do(_ => _.ByLabel.MiddleName, x =>
                    {
                        x.Should.Equal("mdname");
                        x.Set("md2name");
                        x.Should.Not.Equal("mdname");
                    }).
                ById.MiddleName.Should.Equal("md2name").

                ByLabel.ReadonlyField.Should.Exist().
                ByLabel.ReadonlyField.Should.BeEnabled().
                ByLabel.ReadonlyField.Should.BeReadOnly().
                ByLabel.ReadonlyField.IsReadOnly.Should.BeTrue().
                ByLabel.ReadonlyField.Attributes.ReadOnly.Should.BeTrue().
                ByLabel.ReadonlyField.Should.Equal("readme").

                ByLabel.DisabledField.Should.Exist().
                ByLabel.DisabledField.Should.Not.BeEnabled().
                ByLabel.DisabledField.IsEnabled.Should.Not.BeTrue().
                ByLabel.DisabledField.Attributes.Disabled.Should.BeTrue().
                ByLabel.DisabledField.Should.Not.BeReadOnly().
                ByLabel.DisabledField.Should.Equal("readme").

                RawButtonControl.Should.Exist().
                RawButtonControl.Should.BeEnabled().
                RawButtonControl.Content.Should.Equal("Raw Button").
                RawButtonControl.Click().
                InputButtonControl.Should.Exist().
                InputButtonControl.Click().
                Do(_ => _.LinkButtonControl, x =>
                    {
                        x.Should.Exist();
                        x.Content.Should.Equal("Link Button");
                        x.Content.Should.StartWith("Link");
                        x.Content.Should.EndWith("utton");
                        x.Content.Should.Contain("ink Butto");
                        x.Content.Should.ContainIgnoringCase("k but");
                        x.Click();
                    }).
                ClickableControl.Should.Exist().
                ClickableControl.Click().
                DisabledButtonControl.Should.Exist().
                DisabledButtonControl.Should.BeDisabled().
                MissingButtonControl.Should.Not.Exist();
        }
    }
}
