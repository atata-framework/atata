﻿using System.Drawing;

namespace Atata.IntegrationTests.Controls;

public class ControlTests : WebDriverSessionTestSuite
{
    protected override bool ReuseDriver => false;

    [Test]
    public void DragAndDrop_WithDragsAndDropsUsingDomEvents() =>
        Go.To<DragAndDropPage>()
            .DropContainer.Items.Should.BeEmpty()
            .DragItems.Items.Should.HaveCount(2)
            .DragItems[x => x.Content == "Drag item 1"].DragAndDropTo(x => x.DropContainer)
            .DragItems[0].DragAndDropTo(x => x.DropContainer)
            .DropContainer.Items.Should.HaveCount(2)
            .DragItems.Items.Should.BeEmpty()
            .DropContainer[1].Content.Should.Be("Drag item 2");

    [Test]
    public void ScrollTo_WithScrollsUsingActionsAttribute()
    {
        CurrentSession.Driver.Manage().Window.Size = new Size(400, 400);

        Go.To<BasicControlsPage>()
            .OptionBWithScrollUsingActions.ScrollTo();

        object? yOffset = CurrentSession.Driver.AsScriptExecutor().ExecuteScript("return window.pageYOffset;");

        Assert.That(yOffset, Is.GreaterThan(200));
    }

    [Test]
    public void ScrollTo_WithScrollsUsingScriptAttribute()
    {
        CurrentSession.Driver.Manage().Window.Size = new Size(400, 400);

        Go.To<BasicControlsPage>()
            .OptionBWithScrollUsingScript.ScrollTo();

        object? yOffset = CurrentSession.Driver.AsScriptExecutor().ExecuteScript("return window.pageYOffset;");

        Assert.That(yOffset, Is.GreaterThan(200));
    }

    [Test]
    public void ClickAndGo() =>
        Go.To<GoTo1Page>()
            .GoTo2Control.ClickAndGo<GoTo2Page>();

    [Test]
    public void Blur() =>
        Go.To<InputPage>()
            .TelInput.Set("123")
            .TelInput.Blur()
            .ActiveControl.DomProperties.Value.Should.Not.Be("123");

    public class IsFocused : WebDriverSessionTestSuite
    {
        [Test]
        public void AfterSet() =>
            Go.To<InputPage>()
                .TelInput.Set("123")
                .TelInput.IsFocused.Should.BeTrue();

        [Test]
        public void AfterFocus() =>
            Go.To<InputPage>()
                .TelInput.Focus()
                .TelInput.IsFocused.Should.BeTrue();

        [Test]
        public void AfterBlur() =>
            Go.To<InputPage>()
                .TelInput.Set("123")
                .TelInput.Blur()
                .TelInput.IsFocused.Should.BeFalse();
    }

    public class ComponentName : WebDriverSessionTestSuite
    {
        [Test]
        public void FromPropertyName() =>
            Go.To<TestPageObject>()
                .NameFromProperty.ComponentName.ToSubject().Should.Be("Name From Property");

        [Test]
        public void FromNameAttribute() =>
            Go.To<TestPageObject>()
                .GetsNameFromNameAttribute.ComponentName.ToSubject().Should.Be("Name From NameAttribute");

        [Test]
        public void FromFindByLabelAttribute() =>
            Go.To<TestPageObject>()
                .GetsNameFromFindByLabelAttribute.ComponentName.ToSubject().Should.Be("Name From FindByLabelAttribute");

        public class TestPageObject : PageObject<TestPageObject>
        {
            public Button<TestPageObject> NameFromProperty { get; private set; }

            [Name("Name From NameAttribute")]
            [Term("Name From TermAttribute")]
            public Button<TestPageObject> GetsNameFromNameAttribute { get; private set; }

            [FindByLabel("Name From FindByLabelAttribute")]
            public TextInput<TestPageObject> GetsNameFromFindByLabelAttribute { get; private set; }
        }
    }
}
