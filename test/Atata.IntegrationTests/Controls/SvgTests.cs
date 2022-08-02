namespace Atata.IntegrationTests.Controls;

public class SvgTests : UITestFixture
{
    private SvgPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<SvgPage>();

    [Test]
    public void SvgRectangle()
    {
        var sut = _page.Svg.Rectangle;

        sut.Should.BeVisible();
    }

    [Test]
    public void SvgPolygon()
    {
        var sut = _page.Svg.Polygon;

        sut.Should.BeVisible();
    }

    [Test]
    public void SvgText()
    {
        var sut = _page.Svg.Text;

        sut.Should.BeVisible();
        sut.Should.Equal("Drag");
    }

    [Test]
    public void SvgEllipse()
    {
        var sut = _page.Svg.Ellipse;

        sut.Should.BeVisible();
    }

    [Test]
    public void SvgPath()
    {
        var sut = _page.Svg.Path;

        sut.Should.BeVisible();
    }

    [Test]
    public void DragAndDropShape()
    {
        var sut = _page.Svg.Rectangle;

        sut.ComponentLocation.X.Get(out int startX);

        sut.DragAndDropTo(x => x.Svg.Polygon);

        sut.ComponentLocation.X.Should.BeGreater(startX);
    }
}
