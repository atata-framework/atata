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

            GoTo<BasicControlsPage>().
                Header.VerifyEquals("Basic Controls").

                ByLabel.FirstName.VerifyExists().
                ByLabel.FirstName.VerifyEnabled().
                ByLabel.FirstName.VerifyNotReadOnly().
                ByLabel.FirstName.SetGenerated(out firstName).
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
                        x.VerifyNotEqual("mdname");
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

                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                RawButton.VerifyContent("Raw Button").
                RawButton.Click().
                InputButton.VerifyExists().
                InputButton.Click().
                Do(_ => _.LinkButton, x =>
                    {
                        x.VerifyExists();
                        x.VerifyContent("Link Button", TermMatch.Equals);
                        x.VerifyContent("Link", TermMatch.StartsWith);
                        x.VerifyContent("utton", TermMatch.EndsWith);
                        x.VerifyContent("ink Butto", TermMatch.Contains);
                        x.Click();
                    }).
                DisabledButton.VerifyExists().
                DisabledButton.VerifyDisabled().
                MissingButton.VerifyMissing();
        }
    }
}
