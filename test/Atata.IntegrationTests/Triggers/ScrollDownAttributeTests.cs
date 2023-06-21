namespace Atata.IntegrationTests.Triggers;

public class ScrollDownAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<ScrollablePage>().BottomText;

        sut.Should.Not.BeVisibleInViewport();
        sut.Metadata.Add(new ScrollDownAttribute(TriggerEvents.BeforeGet));

        sut.Get(out _);
        sut.Should.BeVisibleInViewport();
    }
}
