namespace Atata.Tests
{
    public class BasicControlsTest : TestBase
    {
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
                Do(x => x.ByLabel.MiddleName, x => x.VerifyEquals("mdname"), x => x.Set("md2name")).
                ById.MiddleName.VerifyEquals("md2name").

                RawButton.VerifyExists().
                RawButton.VerifyEnabled().
                RawButton.VerifyContent("Raw Button").
                RawButton.Click().
                InputButton.VerifyExists().
                InputButton.Click().
                LinkButton.VerifyExists().
                LinkButton.VerifyContent("Link", TermMatch.StartsWith).
                LinkButton.Click().
                DisabledButton.VerifyExists().
                DisabledButton.VerifyDisabled().
                MissingButton.VerifyMissing();
        }
    }
}
