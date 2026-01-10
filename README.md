# [Atata](https://atata.io)

[![NuGet](http://img.shields.io/nuget/v/Atata.svg?style=flat)](https://www.nuget.org/packages/Atata/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata.svg)](https://github.com/atata-framework/atata/releases)
[![Build status](https://dev.azure.com/atata-framework/atata/_apis/build/status/atata-ci?branchName=main)](https://dev.azure.com/atata-framework/atata/_build/latest?definitionId=17&branchName=main)
[![Atata Templates](https://img.shields.io/badge/get-Atata_Templates-green.svg?color=4BC21F)](https://marketplace.visualstudio.com/items?itemName=YevgeniyShunevych.AtataTemplates)\
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![X](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://x.com/AtataFramework)

C#/.NET web UI test automation full-featured framework based on Selenium WebDriver.
It uses a fluent page object pattern;
has a built-in logging system;
contains a unique triggers functionality;
has a set of ready-to-use components.
One of the key ideas of the framework is to provide a simple and intuitive syntax for defining and using page objects.
A page object implementation requires as less code as possible.
You can describe a page object class without any methods and only have a set of properties marked with attributes representing page components.

*The package targets .NET 8.0 and .NET Framework 4.6.2.*

- **[What's new in v3.8.0](https://atata.io/blog/2025/12/11/atata-3.8.0-released/)**
- **[Migrating to Atata 3](https://atata.io/upgrade/to-atata-3/)**

## Features

- **WebDriver**.
  Based on [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) and preserves all its features.
- **Page object model**.
  Provides a unique fluent page object pattern, which is easy to implement and maintain.
- **Components**.
  Contains a rich set of ready-to-use [components](https://atata.io/components/) for inputs, tables, lists, etc.
- **Integration**.
  Works on any .NET test engine (e.g. NUnit, xUnit, SpecFlow) as well as on CI systems like Jenkins, GitHub Actions, or TeamCity.
- **Triggers**.
  A bunch of [triggers](https://atata.io/triggers/) to bind with different events to extend component behavior.
- **Verification**.
  A set of fluent assertion methods and triggers for a component and data verification.
- **Configurable**.
  Defines the default component search strategies as well as additional settings. [Atata.Configuration.Json](https://github.com/atata-framework/atata-configuration-json) provides flexible JSON configurations.
- **Reporting/Logging**.
  Built-in customizable logging; screenshots and snapshots capturing functionality.
- **Extensible**.
  [Atata.HtmlValidation](https://github.com/atata-framework/atata-htmlvalidation) adds HTML page validation.
  [Atata.Bootstrap](https://github.com/atata-framework/atata-bootstrap) and [Atata.KendoUI](https://github.com/atata-framework/atata-kendoui) provide extra components.

## Usage

### Page object

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
public void Create()
{
    Login()
        .New()
            .ModalTitle.Should.Be("New User")
            .General.FirstName.SetRandom(out string firstName)
            .General.LastName.SetRandom(out string lastName)
            .General.Email.SetRandom(out string email)
            .General.Office.SetRandom(out Office office)
            .General.Gender.SetRandom(out Gender gender)
            .Save()
        .GetUserRow(email).View()
            .AggregateAssert(x => x
                .Header.Should.Be($"{firstName} {lastName}")
                .Email.Should.Be(email)
                .Office.Should.Be(office)
                .Gender.Should.Be(gender)
                .Birthday.Should.Not.Exist()
                .Notes.Should.Not.Exist());
}
```

## Documentation

Find out more on [Atata Docs](https://atata.io) and on [Getting Started](https://atata.io/getting-started/) page in particular.

### Tutorials

You can also check the following tutorials:

- [Atata - C# Web Test Automation Framework](https://www.codeproject.com/articles/Atata-New-Test-Automation-Framework) - an introduction to Atata Framework.
- [Verification of Page](https://atata.io/tutorials/verification-of-page/) - how to verify web page data using different approaches of Atata Framework.
- [Verification of Validation Messages](https://atata.io/tutorials/verification-of-validation-messages/) - how to verify validation messages on web pages using Atata Framework.
- [Handle Confirmation Popups](https://atata.io/tutorials/handle-confirmation-popups/) - how to handle different confirmation popups using Atata Framework.
- [Multi-Browser Configuration via .runsettings files](https://atata.io/tutorials/multi-browser-configuration-via-runsettings-files/) - how to configure multi-browser tests application using `.runsettings` files.
- [Reporting to Extent Reports](https://atata.io/tutorials/reporting-to-extentreports/) - how to configure Atata reporting to Extent Reports.

## Community

- Slack: [https://atata-framework.slack.com](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
- X: https://x.com/AtataFramework
- Stack Overflow: https://stackoverflow.com/questions/tagged/atata

## Feedback

Any feedback, issues and feature requests are welcome.

If you faced an issue please report it to [Atata Issues](https://github.com/atata-framework/atata/issues),
[ask a question on Stack Overflow](https://stackoverflow.com/questions/ask?tags=atata+csharp) using [atata](https://stackoverflow.com/questions/tagged/atata) tag
or use another [Atata Contact](https://atata.io/contact/) way.

## Contact author

Contact me if you need a help in test automation using Atata Framework, or if you are looking for a quality test automation implementation for your project.

- LinkedIn: https://www.linkedin.com/in/yevgeniy-shunevych
- Email: yevgeniy.shunevych@gmail.com
- Consulting: https://atata.io/consulting/

## Contributing

Check out [Contributing Guidelines](CONTRIBUTING.md) for details.

## SemVer

Atata Framework tries to follow [Semantic Versioning 2.0](https://semver.org/) when possible.
Sometimes Selenium.WebDriver dependency package can contain breaking changes in minor version releases,
so those changes can break Atata as well.
But Atata manages its sources according to SemVer.
Thus backward compatibility is mostly followed and updates within the same major version
(e.g. from 2.1 to 2.2) should not require code changes.

## License

Atata is an open source software, licensed under the Apache License 2.0.
See [LICENSE](LICENSE) for details.
