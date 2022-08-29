using System.Reflection;
using OpenQA.Selenium.Chrome;

namespace Atata.IntegrationTests.Context;

public class AtataContextBuilderTests : UITestFixtureBase
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
            .ConfigureChrome()
                .WithArguments("headless")
            .Build();

        context.Driver.Should().BeOfType<ChromeDriver>();
    }

    [Test]
    public void ConfigureChrome_After_UseChrome_ExtendsSameFactory()
    {
        var builder = AtataContext.Configure()
            .UseChrome()
            .ConfigureChrome();

        builder.BuildingContext.DriverFactories.Should().HaveCount(1);
        builder.BuildingContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Chrome);
    }

    [Test]
    public void ConfigureChrome_After_UseChrome_CreatesNewFactory_WhenOtherAliasIsSpecified()
    {
        var builder = AtataContext.Configure()
            .UseChrome()
            .ConfigureChrome("chrome_other");

        builder.BuildingContext.DriverFactories.Should().HaveCount(2);
        builder.BuildingContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Chrome);
        builder.BuildingContext.DriverFactories[1].Alias.Should().Be("chrome_other");
    }

    [Test]
    public void ConfigureChrome_After_UseFirefox_WithSameAlias_Throws()
    {
        var builder = AtataContext.Configure()
            .UseFirefox()
                .WithAlias("drv");

        Assert.Throws<ArgumentException>(() =>
            builder.ConfigureChrome("drv"));
    }

    [Test]
    public void ConfigureChrome_After_UseChrome_Executes()
    {
        bool isChromeConfigurationInvoked = false;

        AtataContext.Configure()
            .UseChrome()
                .WithArguments("headless")
            .ConfigureChrome()
                .WithOptions(x => isChromeConfigurationInvoked = true)
            .Build();

        isChromeConfigurationInvoked.Should().BeTrue();
    }

    [Test]
    public void Attributes_Global()
    {
        ConfigureBaseAtataContext()
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
        ConfigureBaseAtataContext()
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

        ConfigureBaseAtataContext()
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

        ConfigureBaseAtataContext()
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

        ConfigureBaseAtataContext()
            .Attributes.Component<TablePage>().Add(
                new InvokeDelegateAttribute(() => isDelegateInvoked = true, TriggerEvents.Init))
            .Build();

        Go.To<BasicControlsPage>();

        isDelegateInvoked.Should().BeFalse();
    }

    [Test]
    public void Attributes_Component_PageObject_TargetingChild()
    {
        ConfigureBaseAtataContext()
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
        ConfigureBaseAtataContext()
            .Attributes.Component<Button<BasicControlsPage>>().Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Generic_DoesNotApply()
    {
        ConfigureBaseAtataContext()
            .Attributes.Component<Button<OrdinaryPage>>().Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Type_Generic()
    {
        ConfigureBaseAtataContext()
            .Attributes.Component(typeof(Button<>)).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Type_NonGeneric()
    {
        ConfigureBaseAtataContext()
            .Attributes.Component(typeof(Button<BasicControlsPage>)).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_TypeName()
    {
        ConfigureBaseAtataContext()
            .Attributes.Component("button").Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Property_Expression()
    {
        ConfigureBaseAtataContext()
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
        ConfigureBaseAtataContext()
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
        ConfigureBaseAtataContext()
            .Attributes.Component<BasicControlsPage>()
                .Property("fwefwefwe").Add(new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }
}
