namespace Atata.ExtentReports.IntegrationTests;

using _ = SignInPage;

[Url("signin")]
public class SignInPage : Page<_>
{
    public TextInput<_> Email { get; private set; }

    public PasswordInput<_> Password { get; private set; }
}
