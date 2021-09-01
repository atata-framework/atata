# [Atata](https://atata.io)

[![NuGet](http://img.shields.io/nuget/v/Atata.svg?style=flat)](https://www.nuget.org/packages/Atata/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata.svg)](https://github.com/atata-framework/atata/releases)
[![Build status](https://dev.azure.com/atata-framework/atata/_apis/build/status/atata-ci?branchName=main)](https://dev.azure.com/atata-framework/atata/_build/latest?definitionId=17&branchName=main)
[![Atata Templates](https://img.shields.io/badge/get-Atata_Templates-green.svg?color=4BC21F)](https://marketplace.visualstudio.com/items?itemName=YevgeniyShunevych.AtataTemplates)\
[![Gitter](https://badges.gitter.im/atata-framework/atata.svg)](https://gitter.im/atata-framework/atata)
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![Twitter](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://twitter.com/AtataFramework)

C#/.NET web UI test automation full featured framework based on Selenium WebDriver.
It uses fluent page object pattern.

Supports .NET Framework 4.0+ and .NET Core/Standard 2.0+.

**[What's new in v1.12.0](https://atata.io/blog/2021/09/01/atata-1.12.0-released/)**

## Features

* **WebDriver**. Based on [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) and preserves all its features.
* **Page Object Model**. Provides unique fluent page object pattern that is easy to implement and maintain.
* **Components**. Contains a rich set of ready to use [components](https://atata.io/components/) for inputs, tables, lists, etc.
* **Integration**. Works on any .NET test engine (e.g. NUnit, xUnit, SpecFlow) as well as on CI systems like Jenkins, Azure DevOps or TeamCity.
* **Triggers**. A bunch of [triggers](https://atata.io/triggers/) to bind with different events to extend component behavior.
* **Verification**. A set of fluent assertion methods and triggers for the component and data verification.
* **Configurable**. Defines the default component search strategies as well as additional settings. [Atata.Configuration.Json](https://github.com/atata-framework/atata-configuration-json) provides flexible JSON configurations.
* **Reporting/Logging**. Built-in customizable logging and screenshot capturing functionality.
* **Extensible**. [Atata.Bootstrap](https://github.com/atata-framework/atata-bootstrap) and [Atata.KendoUI](https://github.com/atata-framework/atata-kendoui) packages have a set of ready to use components. Framework supports any kind of extending.

## Usage

### Page Object

Simple sign-in page object for https://demo.atata.io/signin page:

```C#
using Atata;

namespace SampleApp.UITests
{
    using _ = SignInPage;

    [Url("signin")] // Relative URL of the page.
    [VerifyH1] // Verifies that H1 header text equals "Sign In" upon page object initialization.
    public class SignInPage : Page<_>
    {
        [FindByLabel] // Finds <label> element containing "Email" (<label for="email">Email</label>), then finds text <input> element by "id" that equals label's "for" attribute value.
        public TextInput<_> Email { get; private set; }

        [FindById("password")] // Finds password <input> element by id that equals "password" (<input id="password" type="password">).
        public PasswordInput<_> Password { get; private set; }

        [FindByValue(TermCase.Title)] // Finds button element by value that equals "Sign In" (<input value="Sign In" type="submit">).
        public Button<_> SignIn { get; private set; }
    }
}
```

### Test

Usage in the test method:

```C#
[Test]
public void SignIn()
{
    Go.To<SignInPage>()
        .Email.Set("admin@mail.com")
        .Password.Set("abc123")
        .SignIn.Click();
}
```

### Setup

```C#
[SetUp]
public void SetUp()
{
    AtataContext.Configure()
        .UseChrome()
        .UseBaseUrl("https://demo.atata.io/")
        .Build();
}
```

*Find out more on [Atata usage](https://atata.io/getting-started/#usage). Check [atata-framework/atata-samples](https://github.com/atata-framework/atata-samples) for different Atata test scenario samples.*

## Demo

Demo [atata-framework/atata-sample-app-tests](https://github.com/atata-framework/atata-sample-app-tests) UI tests application demonstrates different testing approaches and features of Atata Framework. It covers main Atata features: page navigation, data input and verification, interaction with pop-ups and tables, logging, screenshot capture, etc.

Sample test:

```C#
[Test]
public void User_Create()
{
    string firstName, lastName, email;
    Office office = Office.NewYork;
    Gender gender = Gender.Male;

    Login()
        .New()
            .ModalTitle.Should.Equal("New User")
            .General.FirstName.SetRandom(out firstName)
            .General.LastName.SetRandom(out lastName)
            .General.Email.SetRandom(out email)
            .General.Office.Set(office)
            .General.Gender.Set(gender)
            .Save()
        .Users.Rows[x => x.FirstName == firstName && x.LastName == lastName && x.Email == email && x.Office == office].View()
            .Header.Should.Equal($"{firstName} {lastName}")
            .Email.Should.Equal(email)
            .Office.Should.Equal(office)
            .Gender.Should.Equal(gender)
            .Birthday.Should.Not.Exist()
            .Notes.Should.Not.Exist();
}
```

## Documentation

Find out more on [Atata Docs](https://atata.io) and on [Getting Started](https://atata.io/getting-started/) page in particular.

### Tutorials

You can also check the following tutorials:

* [Atata - C# Web Test Automation Framework](https://www.codeproject.com/Articles/1158365/Atata-New-Test-Automation-Framework) - an introduction to Atata Framework.
* [Verification of Page](https://atata.io/tutorials/verification-of-page/) - how to verify web page data using different approaches of Atata Framework.
* [Verification of Validation Messages](https://atata.io/tutorials/verification-of-validation-messages/) - how to verify validation messages on web pages using Atata Framework.
* [Handle Confirmation Popups](https://atata.io/tutorials/handle-confirmation-popups/) - how to handle different confirmation popups using Atata Framework.
* [Multi-Browser Configuration via Fixture Arguments](https://atata.io/tutorials/multi-browser-configuration-via-fixture-arguments/) - how to configure multi-browser tests application using NUnit fixture arguments.
* [Visual Studio Team Services Configuration](https://atata.io/tutorials/vs-team-services-configuration/) - how to configure Atata test automation build on Visual Studio Team Services using any browser.

## Contact

Feel free to ask any questions regarding Atata Framework.
Any feedback, issues and feature requests are welcome.

You can [ask a question on Stack Overflow](https://stackoverflow.com/questions/ask?tags=atata+csharp) using [atata](https://stackoverflow.com/questions/tagged/atata) tag.

If you faced an issue please report it to [Atata Issues](https://github.com/atata-framework/atata/issues), write to [Atata Gitter](https://gitter.im/atata-framework/atata) or just mail to yevgeniy.shunevych@gmail.com.

### Links

* Stack Overflow: https://stackoverflow.com/questions/tagged/atata
* Slack: [https://atata-framework.slack.com](https://join.slack.com/t/atata-framework/shared_invite/enQtNDMzMzk3OTY5NjgzLTJlNzAyN2E3MzY3MDE4ZGE1ZDQzOGY2NThiYWExZTNkNDc5YjdlNzFjYmUwYjZmNDI2MDJlMGQ3ODNlMDljMzU)
* Atata Issues: https://github.com/atata-framework/atata/issues
* Atata Gitter: https://gitter.im/atata-framework/atata
* Twitter: https://twitter.com/AtataFramework
* YouTube: https://www.youtube.com/channel/UCSNfv8sKpUR3a6dqPVy54KQ

### Author

Contact me if you need a help in test automation using Atata Framework, or if you are looking for a quality test automation implementation for your project.

* Email: yevgeniy.shunevych@gmail.com
* Skype: e.shunevich
* LinkedIn: https://www.linkedin.com/in/yevgeniy-shunevych
* Gitter: https://gitter.im/YevgeniyShunevych

## SemVer

Atata Framework follows [Semantic Versioning 2.0](https://semver.org/).
Thus backward compatibility is followed and updates within the same major version
(e.g. from 1.3 to 1.4) should not require code changes.

## License

Atata is an open source software, licensed under the Apache License 2.0.
See [LICENSE](LICENSE) for details.
