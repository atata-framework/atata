# [Atata](https://atata.io)

[![NuGet](http://img.shields.io/nuget/v/Atata.svg?style=flat)](https://www.nuget.org/packages/Atata/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata.svg)](https://github.com/atata-framework/atata/releases)
[![Build status](https://dev.azure.com/atata-framework/atata/_apis/build/status/atata-ci?branchName=main)](https://dev.azure.com/atata-framework/atata/_build/latest?definitionId=17&branchName=main)
[![Atata Templates](https://img.shields.io/badge/get-Atata_Templates-green.svg?color=4BC21F)](https://marketplace.visualstudio.com/items?itemName=YevgeniyShunevych.AtataTemplates)\
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![X](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://x.com/AtataFramework)

**Atata Framework** is a full-featured C#/.NET test automation framework built around a powerful context-driven architecture and session-based execution model.
It provides an intuitive, fluent page object pattern for web UI testing, which remains its core capability,
while Atata 4 expands beyond web UI automation into a universal, extensible testing ecosystem.
Designed to minimize boilerplate, Atata enables clean, declarative test components using properties, attributes, and reusable building blocks.
With customizable built-in logging, a unique event-driven trigger system, and a rich ecosystem of ready-to-use components,
Atata provides a consistent foundation for building maintainable and scalable automated tests across different testing domains.

- **[What's new in v4.0.0](https://atata.io/blog/2026/09/07/atata-framework-4-release/)**
- **[Migrating to Atata 4](https://atata.io/upgrade/to-atata-4/)**

*The package targets .NET 8.0 and .NET Framework 4.6.2.*

## Features

- **WebDriver**.
  Provides [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) session functionality.
  Preserves all WebDriver capabilities for local, remote, and headless browser automation.
- **Page object model**.
  Provides a unique fluent page object pattern for concise, maintainable page and component definitions.
- **Components**.
  Includes a rich set of ready-to-use UI testing [components](https://atata.io/components/) for inputs, tables, lists, etc.
- **Smart verification**.
  Offers fluent assertions, aggregate checks, wait-aware verification, and built-in verification triggers.
- **Triggers**.
  Includes a set of [triggers](https://atata.io/triggers/) to bind with different events to extend component behavior.
- **Configuration**.
  Enables flexible settings, variables, URL templates, environment-aware run options, etc.
- **Logging and reporting**.
  Built-in customizable structured logging, screenshots, page snapshots, artifact generation, and extensible log consumer support.
- **Extensible ecosystem**.
  Features a bunch of add-ons such as [Atata.HtmlValidation](https://github.com/atata-framework/atata-htmlvalidation),
  [Atata.NLog](https://github.com/atata-framework/atata-nlog),
  [Atata.AspNetCore](https://github.com/atata-framework/atata-aspnetcore),
  [Atata.Testcontainers](https://github.com/atata-framework/atata-testcontainers).
- **Integration**. 
  Easily integrates with NUnit, xUnit, MSTest, Reqnroll via dedicated packages.
  Works flawlessly on CI systems like GitHub Actions, Jenkins, etc.

## Usage

### Page object

Simple sign-in page object for https://demo.atata.io/signin page:

```C#
using Atata;

namespace SampleApp.UITests;

using _ = SignInPage;

[Url("/signin")] // Relative URL of the page.
public class SignInPage : Page<_>
{
    [FindByLabel] // Finds <label> element containing "Email" (<label for="email">Email</label>), then finds text <input> element by "id" that equals label's "for" attribute value.
    public TextInput<_> Email { get; private set; }

    [FindById("password")] // Finds password <input> element by id that equals "password" (<input id="password" type="password">).
    public PasswordInput<_> Password { get; private set; }

    [FindByValue(TermCase.Title)] // Finds button element by value that equals "Sign In" (<input value="Sign In" type="submit">).
    public Button<_> SignIn { get; private set; }
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

*Find out more on [Atata usage](https://atata.io/getting-started/#usage). Check [atata-framework/atata-samples](https://github.com/atata-framework/atata-samples) repository for different Atata test scenario samples.*

## Demo

Demo [atata-framework/atata-sample-app-tests](https://github.com/atata-framework/atata-sample-app-tests) UI tests application demonstrates different testing approaches and features of Atata Framework.
It covers main Atata features: page navigation, data input and verification, interaction with pop-ups and tables, logging, screenshot capture, etc.

Sample test:

```C#
[Test]
public void Create() =>
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
                .Birthday.Should.Not.BeVisible()
                .Notes.Should.Not.BeVisible());
```

## Documentation

Find out more on [Atata Docs](https://atata.io) and on [Getting Started](https://atata.io/getting-started/) page in particular.

### Tutorials

You may also find the following tutorials helpful:

- [Basic web UI test project](https://atata.io/tutorials/basic-web-ui-test-project/)\
  How to create a basic web UI test project with a workflow test using Atata Framework
- [Verification of page](https://atata.io/tutorials/verification-of-page/)\
  How to verify a web page data using different approaches of Atata Framework.
- [Verification of validation messages](https://atata.io/tutorials/verification-of-validation-messages/)\
  How to verify validation messages on web pages using Atata Framework.
- [Handle confirmation popups](https://atata.io/tutorials/handle-confirmation-popups/)\
  How to handle different confirmation popups using Atata Framework.
- [Complex configuration](https://atata.io/tutorials/complex-configuration/)\
  How to configure multi-environment tests application using environment variables, *.json* and *.runsettings* files.
- [Multi-browser configuration via .runsettings files](https://atata.io/tutorials/multi-browser-configuration-via-runsettings-files/)\
  How to configure multi-browser tests application using *.runsettings* files.
- [Reporting to ExtentReports](https://atata.io/tutorials/reporting-to-extentreports/)\
  How to configure Atata reporting to ExtentReports.

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

Contact me, Yevhenii Shunevych, if you need help with test automation using the Atata Framework.
You can [hire me for test automation development or consulting](https://atata.io/consulting/) if you are looking for a high-quality, maintainable automation solution for your project.

- LinkedIn: https://www.linkedin.com/in/yevgeniy-shunevych
- Email: yevgeniy.shunevych@gmail.com
- Consulting: https://atata.io/consulting/

## Sponsorship

Many thanks to the sponsors that regularly support the development of Atata Framework through donations:

- **[Lombiq Technologies](https://lombiq.com/)**

If Atata Framework is useful to you or your company, consider supporting the framework development with a [donation](https://atata.io/donate/).

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
