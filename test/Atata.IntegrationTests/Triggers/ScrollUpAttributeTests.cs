namespace Atata.IntegrationTests.Triggers;

public class ScrollUpAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<ScrollablePage>()
            .ScrollDown()
            .TopText;

        sut.Should.Not.BeVisibleInViewPort();
        sut.Metadata.Add(new ScrollUpAttribute(TriggerEvents.BeforeGet));

        sut.Get(out _);
        sut.Should.BeVisibleInViewPort();
    }
}
