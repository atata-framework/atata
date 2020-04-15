using NUnit.Framework;

namespace Atata.Tests
{
    public class FindingInAncestorTests : UITestFixture
    {
        private FindingInAncestorPage page;

        protected override void OnSetUp()
        {
            page = Go.To<FindingInAncestorPage>();
        }

        [Test]
        public void Find_InAncestor_First_Visible()
        {
            var control = page.LegendInOneLayer;

            control.Should.Equal("Radio Buttons");
        }

        [Test]
        public void Find_InAncestor_ThreeLayers()
        {
            var control = page.LegendInThreeLayers;

            control.Should.Equal("Radio Buttons");
        }

        [Test]
        public void Find_InAncestor_TreeLayers_AtDifferentLevels()
        {
            var control = page.LegendInThreeLayersAtDifferentLevels;

            control.Should.Equal("Radio Buttons");
        }
    }
}
