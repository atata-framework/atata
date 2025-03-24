namespace Atata.ExtentReports.IntegrationTests;

[BuildSessionAndShare(typeof(WebDriverSession))]
public sealed class UsingSameDriverTests : AtataTestSuite
{
    [OneTimeSetUp]
    public void SetUpFixture() =>
        Go.To<SignInPage>();

    [Test]
    public void Email() =>
        Go.On<SignInPage>()
            .Email.Should.BeVisible();

    [Test]
    public void Password() =>
        Go.On<SignInPage>()
            .Password.Should.BeVisible();
}
