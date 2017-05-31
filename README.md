# [Atata](https://atata-framework.github.io/)

[![NuGet](http://img.shields.io/nuget/v/Atata.svg?style=flat)](https://www.nuget.org/packages/Atata/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata.svg)](https://github.com/atata-framework/atata/releases)
[![Gitter](https://badges.gitter.im/atata-framework/atata.svg)](https://gitter.im/atata-framework/atata)

C#/.NET test automation full featured framework based on Selenium WebDriver. It uses fluent page object pattern.

## Features

* **WebDriver**. Based on [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) and preserves all its features.
* **Page Object**. Provides unique fluent page object pattern that is easy to implement and maintain.
* **Components**. Supports a rich set of HTML [components](https://atata-framework.github.io/components/).
* **Integration**. Works on any .NET test engine (e.g. NUnit, xUnit) as well as on CI systems like TeamCity or TFS.
* **Triggers**. A bunch of [triggers](https://atata-framework.github.io/triggers/) to bind with different component events.
* **Verification**. A set of methods and triggers for the component and data verification.
* **Configurable**. Defines the default component search strategies as well as additional settings.
* **Logging**. Built-in customizable logging and screenshot capturing functionality.
* **Extensible**. [Atata.Bootstrap](https://github.com/atata-framework/atata-bootstrap) and [Atata.KendoUI](https://github.com/atata-framework/atata-kendoui) packages have a set of ready to use components.

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

## Demo

[The Demo Project](https://github.com/atata-framework/atata-sample-app-tests) tests application demonstrates different testing approaches and features of the Atata Framework. It covers main Atata features: page navigation, data input and verification, interaction with pop-ups and tables, logging, screenshot capture, etc.

Sample test:

```C#
[Test]
public void User_Create()
{
    string firstName, lastName, email;
    Office office = Office.NewYork;
    Gender gender = Gender.Male;

    Login().
        New().
            ModalTitle.Should.Equal("New User").
            General.FirstName.SetRandom(out firstName).
            General.LastName.SetRandom(out lastName).
            General.Email.SetRandom(out email).
            General.Office.Set(office).
            General.Gender.Set(gender).
            Save().
        Users.Rows[x => x.FirstName == firstName && x.LastName == lastName && x.Email == email && x.Office == office].View().
            Header.Should.Equal($"{firstName} {lastName}").
            Email.Should.Equal(email).
            Office.Should.Equal(office).
            Gender.Should.Equal(gender).
            Birthday.Should.Not.Exist().
            Notes.Should.Not.Exist();
}
```

## Documentation

Find out more on [Atata Docs](https://atata-framework.github.io/) and on [Getting Started](https://atata-framework.github.io/getting-started/) page in particular.

## Articles

You can also check the following articles:

* [Atata - New Test Automation Framework](https://www.codeproject.com/Articles/1158365/Atata-New-Test-Automation-Framework)
* [Test Automation Using Atata: Verification of Web Pages](https://www.codeproject.com/Articles/1173435/Test-Automation-Using-Atata-Verification-of-Pages)
* [Test Automation Using Atata: Verification of Validation Messages](https://www.codeproject.com/Articles/1177317/Test-Automation-Using-Atata-Validation-Messages)

## Contact

Feel free to ask any questions regarding Atata Framework. Any feedback, issues and feature requests are welcome.

* Atata Issues: https://github.com/atata-framework/atata/issues
* Atata Gitter: https://gitter.im/atata-framework/atata
* Stack Overflow http://stackoverflow.com/questions/tagged/atata

### Author

Contact me if you need a help in test automation using Atata Framework, or if you are looking for a quality test automation implementation for your project.

* Email: yevgeniy.shunevych@gmail.com
* Skype: e.shunevich
* LinkedIn: https://www.linkedin.com/in/yevgeniy-shunevych
* Facebook: https://www.facebook.com/YevgeniyShunevych
* Gitter: https://gitter.im/YevgeniyShunevych

## License

Atata is an open source software, licensed under the Apache License 2.0. See [LICENSE](LICENSE) for details.