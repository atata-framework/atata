namespace Atata.IntegrationTests.Triggers;

public sealed class WaitForDomMutationEndAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute_WhenOnInit_AtPageObject()
    {
        // Arrange
        DynamicListRemovingPage page = new();
        page.Metadata.Push(
            new WaitForDomMutationEndAttribute(TriggerEvents.Init)
            {
                ControlName = nameof(DynamicListRemovingPage.ItemsContainer)
            });

        // Act
        Go.To(page);

        // Assert
        page.Items.Should.AtOnce.HaveCount(5);
    }

    [Test]
    public void Execute_WhenOnInit_AtControl()
    {
        // Arrange
        DynamicListRemovingPage page = new();
        page.ItemsContainer.Metadata.Push(new WaitForDomMutationEndAttribute(TriggerEvents.Init));

        // Act
        Go.To(page);

        // Assert
        page.Items.Should.AtOnce.HaveCount(5);
    }

    [Test]
    public void Execute_WhenAfterClick()
    {
        // Arrange
        DynamicListRemovingPage page = new();
        page.ClickTarget.Metadata.Push(
            new WaitForDomMutationEndAttribute(TriggerEvents.AfterClick)
            {
                ControlName = nameof(DynamicListRemovingPage.ItemsContainer)
            });

        // Act
        Go.To(page)
            .ClickTarget.Click();

        // Assert
        page.Items.Should.AtOnce.HaveCount(5);
    }
}
