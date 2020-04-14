using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    public class FindingInShadowDomTests : UITestFixture
    {
        private ShadowDomPage page;

        protected override void OnSetUp()
        {
            page = Go.To<ShadowDomPage>();
        }

        [Test]
        public void Find_InShadowHost_First_AnyVisibility()
        {
            var control = page.Shadow1_0;

            control.Should.Exist();
            control.Content.Should.BeEmpty();
            control.Scope.TagName.Should().Be("style");
        }

        [Test]
        public void Find_InShadowHost_First_Visible()
        {
            var control = page.Shadow1_1;

            control.Should.Exist();
            control.Content.Should.Equal("Shadow 1.1");
        }

        [Test]
        public void Find_InShadowHost_ByIndex()
        {
            var control = page.Shadow1_2;

            control.Should.Exist();
            control.Content.Should.Equal("Shadow 1.2");
        }

        [Test]
        public void Find_InShadowHost_TwoLayers()
        {
            var control = page.Shadow2_1_1;

            control.Should.Exist();
            control.Content.Should.Equal("2.1.1");
        }

        [Test]
        public void Find_InShadowHost_TreeLayers()
        {
            var control = page.Shadow2_1_1_1;

            control.Should.Exist();
            control.Content.Should.Equal("Shadow 2.1.1.1");
        }

        [Test]
        public void Find_InShadowHost_TreeLayers_AtDifferentLevels()
        {
            var control = page.Shadow2_1_1_1_AtDifferentLevels;

            control.Should.Exist();
            control.Content.Should.Equal("Shadow 2.1.1.1");
        }

        [Test]
        public void Find_InShadowHost_TreeLayers_AtDifferentLevels_WithSetLayers()
        {
            var control = page.Shadow2_1_1_1_AtDifferentLevelsWithSetLayers;

            control.Should.Exist();
            control.Content.Should.Equal("Shadow 2.1.1.1");
        }

        [Test]
        public void Find_InShadowHost_MixedLayers_AtDifferentLevels_WithSetLayers()
        {
            var control = page.Shadow2_1_1_1_MixedAtDifferentLevelsWithSetLayers;

            control.Should.Exist();
            control.Content.Should.Equal("2.1.1.1");
        }
    }
}
