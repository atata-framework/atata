using NUnit.Framework;

namespace Atata.Tests
{
    public class ImageTests : UITestFixture
    {
        private ImagePage page;

        protected override void OnSetUp()
        {
            page = Go.To<ImagePage>();
        }

        [Test]
        public void Image_Loaded()
        {
            var control = page.LoadedImage;

            control.Source.Should.EndWith("/Images/350x150.png");
            control.IsLoaded.Should.BeTrue();
        }

        [Test]
        public void Image_NotLoaded()
        {
            var control = page.NotLoadedImage;

            control.Source.Should.EndWith("/Images/missing.png");
            control.IsLoaded.Should.BeFalse();
        }
    }
}
