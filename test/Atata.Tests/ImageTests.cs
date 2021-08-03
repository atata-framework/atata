using NUnit.Framework;

namespace Atata.Tests
{
    public class ImageTests : UITestFixture
    {
        private ImagePage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<ImagePage>();
        }

        [Test]
        public void Image_Loaded()
        {
            var control = _page.LoadedImage;

            control.Source.Should.EndWith("/images/350x150.png");
            control.IsLoaded.Should.BeTrue();
        }

        [Test]
        public void Image_NotLoaded()
        {
            var control = _page.NotLoadedImage;

            control.Source.Should.EndWith("/images/missing.png");
            control.IsLoaded.Should.BeFalse();
        }
    }
}
