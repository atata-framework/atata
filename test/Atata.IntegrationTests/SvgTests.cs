using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class SvgTests : UITestFixture
    {
        private SvgPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<SvgPage>();
        }

        [Test]
        public void SvgRectangle()
        {
            var control = _page.Svg.Rectangle;

            control.Should.BeVisible();
        }

        [Test]
        public void SvgPolygon()
        {
            var control = _page.Svg.Polygon;

            control.Should.BeVisible();
        }

        [Test]
        public void SvgText()
        {
            var control = _page.Svg.Text;

            control.Should.BeVisible();
            control.Should.Equal("Drag");
        }

        [Test]
        public void SvgEllipse()
        {
            var control = _page.Svg.Ellipse;

            control.Should.BeVisible();
        }

        [Test]
        public void SvgPath()
        {
            var control = _page.Svg.Path;

            control.Should.BeVisible();
        }

        [Test]
        public void Svg_DragAndDropShape()
        {
            var control = _page.Svg.Rectangle;

            control.ComponentLocation.X.Get(out int startX);

            control.DragAndDropTo(x => x.Svg.Polygon);

            control.ComponentLocation.X.Should.BeGreater(startX);
        }
    }
}
