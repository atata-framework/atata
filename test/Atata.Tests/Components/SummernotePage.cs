using _ = Atata.Tests.SummernotePage;

namespace Atata.Tests
{
    [Url("controls/summernote")]
    [VerifyTitle]
    public class SummernotePage : Page<_>
    {
        public ContentEditor<_> EditorAsContentEditor { get; private set; }
    }
}
