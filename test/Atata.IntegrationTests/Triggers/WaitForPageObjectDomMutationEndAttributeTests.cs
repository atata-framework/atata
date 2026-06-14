namespace Atata.IntegrationTests.Triggers;

public sealed class WaitForPageObjectDomMutationEndAttributeTests : UITestFixture
{
    [Test]
    public void Execute_WhenOnInit_AtPageObject()
    {
        // Arrange
        DynamicListRemovingPage page = new();
        page.Metadata.Push(new WaitForPageObjectDomMutationEndAttribute(TriggerEvents.Init));

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
        page.ItemsContainer.Metadata.Push(new WaitForPageObjectDomMutationEndAttribute(TriggerEvents.Init));

        // Act
        Go.To(page);

        // Assert
        page.Items.Should.AtOnce.HaveCount(5);
    }
}
