namespace Atata.IntegrationTests.Controls;

public class ImageTests : UITestFixture
{
    private ImagePage _page;

    protected override void OnSetUp() =>
        _page = Go.To<ImagePage>();

    [Test]
    public void WhenLoaded()
    {
        var sut = _page.LoadedImage;

        sut.Source.Should.EndWith("/images/350x150.png");
        sut.IsLoaded.Should.BeTrue();
    }

    [Test]
    public void WhenNotLoaded()
    {
        var sut = _page.NotLoadedImage;

        sut.Source.Should.EndWith("/images/missing.png");
        sut.IsLoaded.Should.BeFalse();
    }
}
