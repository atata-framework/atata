# [Atata](https://atata.io)

[![NuGet](http://img.shields.io/nuget/v/Atata.svg?style=flat)](https://www.nuget.org/packages/Atata/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata.svg)](https://github.com/atata-framework/atata/releases)
[![Build status](https://dev.azure.com/atata-framework/atata/_apis/build/status/atata-ci?branchName=main)](https://dev.azure.com/atata-framework/atata/_build/latest?definitionId=17&branchName=main)
[![Atata Templates](https://img.shields.io/badge/get-Atata_Templates-green.svg?color=4BC21F)](https://marketplace.visualstudio.com/items?itemName=YevgeniyShunevych.AtataTemplates)\
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![X](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://x.com/AtataFramework)

基于 Selenium WebDriver 的 C#/.NET Web UI 测试自动化全功能框架。
采用流畅的页面对象模式；
内置日志系统；
包含独特的触发器功能；
提供一组即用型组件。
框架的核心思想之一是为定义和使用页面对象提供简单直观的语法。
页面对象的实现需要尽可能少的代码。
你可以在不编写任何方法的情况下描述一个页面对象类，仅通过一组标记了属性的属性来表示页面组件。

- **[v3.11.0 新特性](https://atata.io/blog/2026/05/18/atata-3.11.0-released/)**
- **[迁移到 Atata 3](https://atata.io/upgrade/to-atata-3/)**

*该包面向 .NET Standard 2.0，支持 .NET 5+、.NET Framework 4.6.1+ 和 .NET Core/Standard 2.0+。*

## 特性

- **WebDriver**。
  基于 [Selenium WebDriver](https://github.com/SeleniumHQ/selenium) 并保留其所有功能。
- **页面对象模型**。
  提供独特的流畅页面对象模式，易于实现和维护。
- **组件**。
  包含丰富的即用型[组件](https://atata.io/components/)，用于输入框、表格、列表等。
- **集成**。
  可在任何 .NET 测试引擎（如 NUnit、xUnit、SpecFlow）以及 CI 系统（如 Jenkins、GitHub Actions、TeamCity）上运行。
- **触发器**。
  一组[触发器](https://atata.io/triggers/)，可与不同事件绑定以扩展组件行为。
- **验证**。
  一组流畅的断言方法和触发器，用于组件和数据验证。
- **可配置**。
  定义默认的组件搜索策略以及其他设置。[Atata.Configuration.Json](https://github.com/atata-framework/atata-configuration-json) 提供灵活的 JSON 配置。
- **报告/日志**。
  内置可自定义的日志功能；截图和快照捕获功能。
- **可扩展**。
  [Atata.HtmlValidation](https://github.com/atata-framework/atata-htmlvalidation) 添加 HTML 页面验证。
  [Atata.Bootstrap](https://github.com/atata-framework/atata-bootstrap) 和 [Atata.KendoUI](https://github.com/atata-framework/atata-kendoui) 提供额外组件。

## 使用

### 页面对象

https://demo.atata.io/signin 页面的简单登录页面对象：

```C#
using Atata;

namespace SampleApp.UITests
{
    using _ = SignInPage;

    [Url("signin")] // 页面的相对 URL。
    [VerifyH1] // 在页面对象初始化时验证 H1 标题文本是否等于 "Sign In"。
    public class SignInPage : Page<_>
    {
        [FindByLabel] // 查找包含 "Email" 的 <label> 元素（<label for="email">Email</label>），然后通过标签的 "for" 属性值查找对应的文本 <input> 元素。
        public TextInput<_> Email { get; private set; }

        [FindById("password")] // 通过 id 等于 "password" 查找密码 <input> 元素（<input id="password" type="password">）。
        public PasswordInput<_> Password { get; private set; }

        [FindByValue(TermCase.Title)] // 通过值等于 "Sign In" 查找按钮元素（<input value="Sign In" type="submit">）。
        public Button<_> SignIn { get; private set; }
    }
}
```

### 测试

在测试方法中的用法：

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

### 设置

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

*了解更多请访问 [Atata 使用指南](https://atata.io/getting-started/#usage)。查看 [atata-framework/atata-samples](https://github.com/atata-framework/atata-samples) 获取不同的 Atata 测试场景示例。*

## 演示

演示项目 [atata-framework/atata-sample-app-tests](https://github.com/atata-framework/atata-sample-app-tests) UI 测试应用展示了 Atata 框架的不同测试方法和功能。它涵盖了 Atata 的主要特性：页面导航、数据输入和验证、弹窗和表格交互、日志记录、截图捕获等。

示例测试：

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

## 文档

了解更多请访问 [Atata 文档](https://atata.io)，特别是[入门指南](https://atata.io/getting-started/)页面。

### 教程

你还可以查看以下教程：

- [Atata - C# Web 测试自动化框架](https://www.codeproject.com/articles/Atata-New-Test-Automation-Framework) - Atata 框架入门介绍。
- [页面验证](https://atata.io/tutorials/verification-of-page/) - 如何使用 Atata 框架的不同方法验证网页数据。
- [验证消息验证](https://atata.io/tutorials/verification-of-validation-messages/) - 如何使用 Atata 框架验证网页上的验证消息。
- [处理确认弹窗](https://atata.io/tutorials/handle-confirmation-popups/) - 如何使用 Atata 框架处理不同的确认弹窗。
- [通过 .runsettings 文件配置多浏览器](https://atata.io/tutorials/multi-browser-configuration-via-runsettings-files/) - 如何使用 `.runsettings` 文件配置多浏览器测试应用。
- [报告到 Extent Reports](https://atata.io/tutorials/reporting-to-extentreports/) - 如何配置 Atata 报告到 Extent Reports。

## 社区

- Slack: [https://atata-framework.slack.com](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
- X: https://x.com/AtataFramework
- Stack Overflow: https://stackoverflow.com/questions/tagged/atata

## 反馈

欢迎提供任何反馈、问题和功能请求。

如果你遇到了问题，请在 [Atata Issues](https://github.com/atata-framework/atata/issues) 中报告，
使用 [atata](https://stackoverflow.com/questions/tagged/atata) 标签[在 Stack Overflow 上提问](https://stackoverflow.com/questions/ask?tags=atata+csharp)，
或使用其他 [Atata 联系](https://atata.io/contact/)方式。

## 联系作者

如果你在使用 Atata 框架进行测试自动化方面需要帮助，或者正在为你的项目寻找高质量的测试自动化实现，请联系我。

- LinkedIn: https://www.linkedin.com/in/yevgeniy-shunevych
- Email: yevgeniy.shunevych@gmail.com
- 咨询: https://atata.io/consulting/

## 贡献

详情请查看[贡献指南](CONTRIBUTING.md)。

## 语义版本控制

Atata 框架尽可能遵循[语义化版本 2.0](https://semver.org/)。
有时 Selenium.WebDriver 依赖包在次版本发布中可能包含破坏性变更，
这些变更也可能影响 Atata。
但 Atata 按照语义化版本管理其源代码。
因此基本上遵循向后兼容性，同一主要版本内的更新
（例如从 2.1 到 2.2）不应需要代码修改。

## 许可证

Atata 是开源软件，基于 Apache License 2.0 许可。
详情请参阅 [LICENSE](LICENSE)。
