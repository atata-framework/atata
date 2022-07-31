namespace Atata.IntegrationTests;

using _ = SvgPage;

[Url("svg")]
[VerifyTitle("SVG")]
public class SvgPage : Page<_>
{
    public SvgControl Svg { get; private set; }

    public class SvgControl : Svg<_>
    {
        [FindByClass("draggable")]
        public SvgRectangle<_> Rectangle { get; private set; }

        [FindByClass("draggable")]
        public SvgPolygon<_> Polygon { get; private set; }

        public SvgText<_> Text { get; private set; }

        public SvgEllipse<_> Ellipse { get; private set; }

        public SvgPath<_> Path { get; private set; }
    }
}
