using System.Reflection;
using OpenQA.Selenium.Chrome;

namespace Atata.IntegrationTests.Context;

public class AtataContextBuilderTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void Build_WithoutDriver()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
           AtataContext.Configure().Build());

        Assert.That(exception.Message, Does.Contain("no driver is specified"));
    }

    [Test]
    public void Build_WithoutUseDriverButWithConfigureChrome()
    {
        var context = AtataContext.Configure()
            .Sessions.AddWebDriver(x => x
                .ConfigureChrome()
                    .WithArguments(ChromeArguments))
            .Build();

        context.GetWebDriverSession().Driver.Should().BeOfType<ChromeDriver>();
    }

    [Test]
    public void ConfigureChrome_After_UseChrome_ExtendsSameFactory()
    {
        var builder = AtataContext.Configure()
            .Sessions.AddWebDriver(x =>
            {
                x.UseChrome();
                x.ConfigureChrome();
            });

        var sessionBuilder = builder.Sessions.Builders.OfType<WebDriverSessionBuilder>().Single();
        sessionBuilder.DriverFactories.Should().HaveCount(1);
        sessionBuilder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Chrome);
    }

    [Test]
    public void ConfigureChrome_After_UseChrome_CreatesNewFactory_WhenOtherAliasIsSpecified()
    {
        var builder = AtataContext.Configure()
            .Sessions.AddWebDriver(x =>
            {
                x.UseChrome();
                x.ConfigureChrome("chrome_other");
            });

        var sessionBuilder = builder.Sessions.Builders.OfType<WebDriverSessionBuilder>().Single();
        sessionBuilder.DriverFactories.Should().HaveCount(2);
        sessionBuilder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Chrome);
        sessionBuilder.DriverFactories[1].Alias.Should().Be("chrome_other");
    }

    [Test]
    public void ConfigureChrome_After_UseFirefox_WithSameAlias_Throws() =>
        AtataContext.Configure()
            .Sessions.AddWebDriver(x =>
            {
                x.UseFirefox()
                    .WithAlias("drv");

                Assert.Throws<ArgumentException>(() =>
                    x.ConfigureChrome("drv"));
            });

    [Test]
    public void ConfigureChrome_After_UseChrome_Executes()
    {
        bool isChromeConfigurationInvoked = false;

        AtataContext.Configure()
            .Sessions.AddWebDriver(x =>
            {
                x.UseChrome()
                    .WithArguments(ChromeArguments);
                x.ConfigureChrome()
                    .WithOptions(_ => isChromeConfigurationInvoked = true);
            })
            .Build();

        isChromeConfigurationInvoked.Should().BeTrue();
    }

    [Test]
    public void UseDisposeDriver_WithTrue()
    {
        var context = BuildAtataContextWithWebDriverSession(
            x => x.UseDisposeDriver(true));
        var driver = context.GetWebDriver();

        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() =>
            _ = driver.Url);
    }

    [Test]
    public void UseDisposeDriver_WithFalse()
    {
        var context = BuildAtataContextWithWebDriverSession(
            x => x.UseDisposeDriver(false));
        var driver = context.GetWebDriver();

        context.Dispose();

        Assert.DoesNotThrow(() =>
            _ = driver.Url);

        driver.Dispose();
    }

    [Test]
    public void Attributes_Global()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Global.Add(
                new FindByContentAttribute("_missing_")
                {
                    TargetParentType = typeof(BasicControlsPage),
                    TargetName = nameof(BasicControlsPage.MissingButtonControl)
                },
                new FindByContentAttribute("Raw Button")
                {
                    TargetParentType = typeof(BasicControlsPage),
                    TargetName = nameof(BasicControlsPage.MissingButtonControl)
                })
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Assembly()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Assembly(Assembly.GetAssembly(GetType())).Add(
                new FindByContentAttribute("_missing_")
                {
                    TargetParentType = typeof(BasicControlsPage),
                    TargetName = nameof(BasicControlsPage.MissingButtonControl)
                },
                new FindByContentAttribute("Raw Button")
                {
                    TargetParentType = typeof(BasicControlsPage),
                    TargetName = nameof(BasicControlsPage.MissingButtonControl)
                })
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_PageObject()
    {
        bool isDelegateInvoked = false;

        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<BasicControlsPage>().Add(
                new InvokeDelegateAttribute(() => isDelegateInvoked = true, TriggerEvents.Init))
            .Build();

        Go.To<BasicControlsPage>();

        isDelegateInvoked.Should().BeTrue();
    }

    [Test]
    public void Attributes_Component_PageObject_Base()
    {
        bool isDelegateInvoked = false;

        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component(typeof(Page<>)).Add(
                new InvokeDelegateAttribute(() => isDelegateInvoked = true, TriggerEvents.Init))
            .Build();

        Go.To<StubPage>();

        isDelegateInvoked.Should().BeTrue();
    }

    [Test]
    public void Attributes_Component_PageObject_DoesNotApply()
    {
        bool isDelegateInvoked = false;

        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<TablePage>().Add(
                new InvokeDelegateAttribute(() => isDelegateInvoked = true, TriggerEvents.Init))
            .Build();

        Go.To<BasicControlsPage>();

        isDelegateInvoked.Should().BeFalse();
    }

    [Test]
    public void Attributes_Component_PageObject_TargetingChild()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<BasicControlsPage>().Add(
                new FindByContentAttribute("_missing_")
                {
                    TargetName = nameof(BasicControlsPage.MissingButtonControl)
                },
                new FindByContentAttribute("Raw Button")
                {
                    TargetName = nameof(BasicControlsPage.MissingButtonControl)
                })
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Generic()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<Button<BasicControlsPage>>().Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Generic_DoesNotApply()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<Button<OrdinaryPage>>().Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Type_Generic()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component(typeof(Button<>)).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Type_NonGeneric()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component(typeof(Button<BasicControlsPage>)).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_TypeName()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component("button").Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Property_Expression()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<BasicControlsPage>()
                .Property(x => x.MissingButtonControl).Add(
                    new FindByContentAttribute("_missing_"),
                    new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Property_Name()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<BasicControlsPage>()
                .Property(nameof(BasicControlsPage.MissingButtonControl)).Add(
                    new FindByContentAttribute("_missing_"),
                    new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Property_Name_DoesNotApply()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<BasicControlsPage>()
                .Property("fwefwefwe").Add(new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }
}
