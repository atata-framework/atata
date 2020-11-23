using _ = Atata.Tests.TinyMcePage;

namespace Atata.Tests
{
    [Url("controls/tinymce")]
    [VerifyTitle("TinyMCE")]
    public class TinyMcePage : Page<_>
    {
        public FrameWrappedContentEditor<_> EditorAsFrameWrappedContentEditor { get; private set; }
    }
}
