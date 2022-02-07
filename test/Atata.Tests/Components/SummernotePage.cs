namespace Atata.Tests
{
    using _ = SummernotePage;

    [Url("controls/summernote")]
    [VerifyTitle]
    public class SummernotePage : Page<_>
    {
        public ContentEditor<_> EditorAsContentEditor { get; private set; }
    }
}
