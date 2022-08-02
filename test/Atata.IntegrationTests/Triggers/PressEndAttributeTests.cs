namespace Atata.IntegrationTests.Triggers;

public class PressEndAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<ScrollablePage>().BottomText;

        sut.Should.Not.BeVisibleInViewPort();
        sut.Metadata.Add(new PressEndAttribute(TriggerEvents.BeforeGet));

        sut.Get(out _);
        sut.Should.BeVisibleInViewPort();
    }
}
