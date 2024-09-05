namespace Atata.IntegrationTests.Triggers;

public class PressEndAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<ScrollablePage>().BottomText;

        sut.Should.Not.BeVisibleInViewport();
        sut.Metadata.Add(new PressEndAttribute(TriggerEvents.BeforeGet));

        sut.Get(out _);
        sut.Should.BeVisibleInViewport();
    }
}
