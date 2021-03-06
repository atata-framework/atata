﻿using NUnit.Framework;

namespace Atata.Tests
{
    public class SvgTests : UITestFixture
    {
        private SvgPage page;

        protected override void OnSetUp()
        {
            page = Go.To<SvgPage>();
        }

        [Test]
        public void SvgRectangle()
        {
            var control = page.Svg.Rectangle;

            control.Should.BeVisible();
        }

        [Test]
        public void SvgPolygon()
        {
            var control = page.Svg.Polygon;

            control.Should.BeVisible();
        }

        [Test]
        public void SvgText()
        {
            var control = page.Svg.Text;

            control.Should.BeVisible();
            control.Should.Equal("Drag");
        }

        [Test]
        public void SvgEllipse()
        {
            var control = page.Svg.Ellipse;

            control.Should.BeVisible();
        }

        [Test]
        public void SvgPath()
        {
            var control = page.Svg.Path;

            control.Should.BeVisible();
        }

        [Test]
        public void Svg_DragAndDropShape()
        {
            var control = page.Svg.Rectangle;

            control.ComponentLocation.X.Get(out int startX);

            control.DragAndDropTo(x => x.Svg.Polygon);

            control.ComponentLocation.X.Should.BeGreater(startX);
        }
    }
}
