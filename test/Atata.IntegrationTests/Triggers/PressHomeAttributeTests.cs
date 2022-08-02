namespace Atata.IntegrationTests.Triggers;

public class PressHomeAttributeTests : UITestFixture
{
    [Test]
    public void Execute()
    {
        var sut = Go.To<ScrollablePage>()
            .Press(Keys.End)
            .TopText;

        sut.Should.Not.BeVisibleInViewPort();
        sut.Metadata.Add(new PressHomeAttribute(TriggerEvents.BeforeGet));

        sut.Get(out _);
        sut.Should.BeVisibleInViewPort();
    }
}
