# [Atata](https://atata-framework.github.io/)

C#/.NET test automation full featured framework based on Selenium WebDriver. It uses fluent page object pattern.

## Demo

[The Demo Project](https://github.com/atata-framework/atata-sample-app-tests) tests application demonstrates different testing approaches and features of the Atata Framework. It covers main Atata features: page navigation, data input and verification, interaction with pop-ups and tables, logging, screenshot capture, etc.

## Features

* **WebDriver**. Based on [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) and preserves all its features.
* **Page Object**. Provides unique fluent page object pattern that is easy to implement and maintain.
* **Components**. Supports a rich set of HTML [components](https://atata-framework.github.io/components/).
* **Integration**. Works on any .NET test engine (e.g. NUnit, xUnit) as well as on CI systems like TeamCity or TFS.
* **Triggers**. A bunch of [triggers](https://atata-framework.github.io/triggers/) to bind with different component events.
* **Verification**. A set of methods and triggers for the component and data verification.
* **Configurable**. Defines the default component search strategies as well as additional settings.
* **Logging**. Built-in customizable logging and screenshot capturing functionality.
* **Extensible**. Atata.Bootstrap and Atata.KendoUI packages with a lot of ready to use components.

## Usage

Simple sign-in page object:

```C#
using Atata;
using _ = SampleApp.AutoTests.SignInPage;

namespace SampleApp.AutoTests
{
    [Url("signin")]
    [VerifyTitle]
    [VerifyH1]
    public class SignInPage : Page<_>
    {
        public TextInput<_> Email { get; private set; }

        public PasswordInput<_> Password { get; private set; }

        public Button<_> SignIn { get; private set; }
    }
}

```

Usage in the test method:

```C#
[Test]
public void SignIn()
{
    Go.To<SignInPage>().
        Email.Set("admin@mail.com").
        Password.Set("abc123").
        SignIn.Click();
}
```

## Documentation

Find out more on [Atata Docs](https://atata-framework.github.io/) and on [Getting Started](https://atata-framework.github.io/getting-started/) page in particular.

## License

Atata is an open source software, licensed under the Apache License 2.0. See [LICENSE](LICENSE) for details.