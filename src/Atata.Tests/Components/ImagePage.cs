namespace Atata.Tests
{
    using _ = ImagePage;

    [Url("image")]
    [VerifyTitle]
    public class ImagePage : Page<_>
    {
        [FindById]
        public Image<_> LoadedImage { get; private set; }

        [FindById]
        public Image<_> NotLoadedImage { get; private set; }
    }
}
