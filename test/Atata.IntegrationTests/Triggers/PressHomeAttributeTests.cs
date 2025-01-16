namespace Atata.IntegrationTests.Triggers;

public class PressHomeAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    [Platform(Exclude = Platforms.MacOS)]
    public void Execute()
    {
        var sut = Go.To<ScrollablePage>()
            .BottomText.WaitTo.Not.BeVisibleInViewport()
            .ScrollDown()
            .TopText;

        sut.Should.Not.BeVisibleInViewport();
        sut.Metadata.Add(new PressHomeAttribute(TriggerEvents.BeforeGet));

        sut.Get(out _);
        sut.Should.BeVisibleInViewport();
    }
}
