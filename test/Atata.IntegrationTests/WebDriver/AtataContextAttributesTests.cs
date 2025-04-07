namespace Atata.IntegrationTests.WebDriver;

public class AtataContextAttributesTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void Global()
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
    public void Assembly()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Assembly(System.Reflection.Assembly.GetAssembly(GetType())!).Add(
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
    public void Component_PageObject()
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
    public void Component_PageObject_Base()
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
    public void Component_PageObject_DoesNotApply()
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
    public void Component_PageObject_TargetingChild()
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
    public void Component_Control_Generic()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<Button<BasicControlsPage>>().Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Component_Control_Generic_DoesNotApply()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<Button<OrdinaryPage>>().Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }

    [Test]
    public void Component_Control_Type_Generic()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component(typeof(Button<>)).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Component_Control_Type_NonGeneric()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component(typeof(Button<BasicControlsPage>)).Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Component_Control_TypeName()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component("button").Add(
                new FindByContentAttribute("_missing_"),
                new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.BeVisible();
    }

    [Test]
    public void Property_Expression()
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
    public void Property_Name()
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
    public void Property_Name_DoesNotApply()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Attributes.Component<BasicControlsPage>()
                .Property("fwefwefwe").Add(new FindFirstAttribute())
            .Build();

        Go.To<BasicControlsPage>().MissingButtonControl.Should.AtOnce.Not.BeVisible();
    }
}
