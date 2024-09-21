using System.Reflection;

namespace Atata.IntegrationTests.Context;

public class AtataContextBuilderTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void Attributes_Global()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Global.Add(
            new FindByContentAttribute("_missing_")
            {
                TargetParentType = typeof(BasicControlsPage),
                TargetName = nameof(BasicControlsPage.MissingButtonControl)
            },
            new FindByContentAttribute("Raw Button")
            {
                TargetParentType = typeof(BasicControlsPage),
                TargetName = nameof(BasicControlsPage.MissingButtonControl)
            });
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Assembly()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Assembly(Assembly.GetAssembly(GetType())).Add(
            new FindByContentAttribute("_missing_")
            {
                TargetParentType = typeof(BasicControlsPage),
                TargetName = nameof(BasicControlsPage.MissingButtonControl)
            },
            new FindByContentAttribute("Raw Button")
            {
                TargetParentType = typeof(BasicControlsPage),
                TargetName = nameof(BasicControlsPage.MissingButtonControl)
            });
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_PageObject()
    {
        bool isDelegateInvoked = false;

        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<BasicControlsPage>().Add(
            new InvokeDelegateAttribute(() => isDelegateInvoked = true, TriggerEvents.Init));
        builder.Build();

        Go.To<BasicControlsPage>();

        isDelegateInvoked.Should().BeTrue();
    }

    [Test]
    public void Attributes_Component_PageObject_Base()
    {
        bool isDelegateInvoked = false;

        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component(typeof(Page<>)).Add(
            new InvokeDelegateAttribute(() => isDelegateInvoked = true, TriggerEvents.Init));
        builder.Build();

        Go.To<StubPage>();

        isDelegateInvoked.Should().BeTrue();
    }

    [Test]
    public void Attributes_Component_PageObject_DoesNotApply()
    {
        bool isDelegateInvoked = false;

        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<TablePage>().Add(
            new InvokeDelegateAttribute(() => isDelegateInvoked = true, TriggerEvents.Init));
        builder.Build();

        Go.To<BasicControlsPage>();

        isDelegateInvoked.Should().BeFalse();
    }

    [Test]
    public void Attributes_Component_PageObject_TargetingChild()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<BasicControlsPage>().Add(
            new FindByContentAttribute("_missing_")
            {
                TargetName = nameof(BasicControlsPage.MissingButtonControl)
            },
            new FindByContentAttribute("Raw Button")
            {
                TargetName = nameof(BasicControlsPage.MissingButtonControl)
            });
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Generic()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<Button<BasicControlsPage>>().Add(
            new FindByContentAttribute("_missing_"),
            new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Generic_DoesNotApply()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<Button<OrdinaryPage>>().Add(
            new FindByContentAttribute("_missing_"),
            new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Type_Generic()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component(typeof(Button<>)).Add(
            new FindByContentAttribute("_missing_"),
            new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_Type_NonGeneric()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component(typeof(Button<BasicControlsPage>)).Add(
            new FindByContentAttribute("_missing_"),
            new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Component_Control_TypeName()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component("button").Add(
            new FindByContentAttribute("_missing_"),
            new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Property_Expression()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<BasicControlsPage>()
            .Property(x => x.MissingButtonControl).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Property_Name()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<BasicControlsPage>()
            .Property(nameof(BasicControlsPage.MissingButtonControl)).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Attributes_Property_Name_DoesNotApply()
    {
        var builder = ConfigureAtataContextWithWebDriverSession();
        builder.Attributes.Component<BasicControlsPage>()
            .Property("fwefwefwe").Add(new FindFirstAttribute());
        builder.Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }
}
