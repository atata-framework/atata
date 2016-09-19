# Atata
C#/.NET test automation framework based on Selenium WebDriver. Uses fluent page object pattern.

## Demo
[Demo Project](https://github.com/atata-framework/atata-sample-app-tests) covers main Atata features usage: page navigation, data input and verification, interaction with pop-ups (Bootstrap modal) and tables, logging, screenshot capture, etc.

## Usage
Simple sign-in page object:
```C#
using _ = Atata.SampleApp.AutoTests.SignInPage;

namespace Atata.SampleApp.AutoTests
{
    [VerifyTitle]
    [VerifyH1]
    [NavigateTo("signin")]
    public class SignInPage : Page<_>
    {
        public TextInput<_> Email { get; private set; }

        public PasswordInput<_> Password { get; private set; }

        public Button<UsersPage, _> SignIn { get; private set; }
    }
}

```
and usage in the test method:
```C#
Go.To<SignInPage>().
    Email.Set("example@mail.com").
    Password.Set("password").
    SignIn();
```

###### More documentation coming soon...