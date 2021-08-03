using NUnit.Framework;

namespace Atata.Tests
{
    public class FindingInAncestorTests : UITestFixture
    {
        private FindingInAncestorPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<FindingInAncestorPage>();
        }

        [Test]
        public void Find_InAncestor_First_Visible()
        {
            var control = _page.LegendInOneLayer;

            control.Should.Equal("Radio Buttons");
        }

        [Test]
        public void Find_InAncestor_ThreeLayers()
        {
            var control = _page.LegendInThreeLayers;

            control.Should.Equal("Radio Buttons");
        }

        [Test]
        public void Find_InAncestor_ThreeLayers_AtParentAndDeclared()
        {
            var control = _page.LegendInThreeLayersAtParentAndDeclared;

            control.Should.Equal("Radio Buttons");
        }

        [Test]
        public void Find_InAncestor_ThreeLayers_AtParentAndDeclaredAndComponent()
        {
            var control = _page.LegendInThreeLayersAtParentAndDeclaredAndComponent;

            control.Should.Equal("Radio Buttons");
        }
    }
}
