namespace Atata.IntegrationTests.Triggers;

public class ScrollToAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<ScrollablePage>().BottomText;

        sut.Should.Not.BeVisibleInViewport();
        sut.Metadata.Add(new ScrollToAttribute(TriggerEvents.BeforeGet));

        sut.Get(out _);
        sut.Should.BeVisibleInViewport();
    }
}
