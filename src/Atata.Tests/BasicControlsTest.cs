using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class BasicControlsTest : TestBase
    {
        [Test]
        public void BasicControlsInteraction()
        {
            string firstName;

            Go.To<BasicControlsPage>().
                Header.VerifyEquals("Basic Controls").

                ByLabel.FirstName.VerifyExists().
                ByLabel.FirstName.VerifyEnabled().
                ByLabel.FirstName.VerifyNotReadOnly().
                ByLabel.FirstName.SetRandom(out firstName).
                ByLabel.FirstName.VerifyEquals(firstName).
                ById.FirstName.VerifyEquals(firstName).

                ByLabel.LastName.Set("LastName").
                ByLabel.LastName.VerifyEquals("LastName").
                ById.LastName.VerifyEquals("LastName").

                ById.MiddleName.Set("mdname").
                Do(_ => _.ByLabel.MiddleName, x =>
                    {
                        x.VerifyEquals("mdname");
                        x.Set("md2name");
                        x.VerifyDoesNotEqual("mdname");
                    }).
                ById.MiddleName.VerifyEquals("md2name").

                ByLabel.ReadonlyField.VerifyExists().
                ByLabel.ReadonlyField.VerifyEnabled().
                ByLabel.ReadonlyField.VerifyReadOnly().
                ByLabel.ReadonlyField.VerifyEquals("readme").

                ByLabel.DisabledField.VerifyExists().
                ByLabel.DisabledField.VerifyDisabled().
                ByLabel.DisabledField.VerifyNotReadOnly().
                ByLabel.DisabledField.VerifyEquals("readme").

                RawButtonControl.VerifyExists().
                RawButtonControl.VerifyEnabled().
                RawButtonControl.Content.VerifyEquals("Raw Button").
                RawButtonControl.Click().
                InputButtonControl.VerifyExists().
                InputButtonControl.Click().
                Do(_ => _.LinkButtonControl, x =>
                    {
                        x.VerifyExists();
                        x.Content.VerifyEquals("Link Button");
                        x.Content.VerifyStartsWith("Link");
                        x.Content.VerifyEndsWith("utton");
                        x.Content.VerifyContains("ink Butto");
                        x.Click();
                    }).
                ClickableControl.VerifyExists().
                ClickableControl.Click().
                DisabledButtonControl.VerifyExists().
                DisabledButtonControl.VerifyDisabled().
                MissingButtonControl.VerifyMissing();
        }
    }
}
