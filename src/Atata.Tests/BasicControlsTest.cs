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
                Header.VerifyEquals("Header 1").

                ByLabel.FirstName.SetGenerated(out firstName).
                ByLabel.FirstName.VerifyEquals(firstName).
                ById.FirstName.VerifyEquals(firstName).

                ByLabel.LastName.Set("LastName").
                ByLabel.LastName.VerifyEquals("LastName").
                ById.LastName.VerifyEquals("LastName").

                ById.MiddleName.Set("mdname").
                Do(_ => _.ByLabel.MiddleName, x => x.VerifyEquals("mdname"), x => x.Set("md2name")).
                ById.MiddleName.VerifyEquals("md2name").

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
