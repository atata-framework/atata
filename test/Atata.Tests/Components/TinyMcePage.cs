namespace Atata.Tests
{
    using _ = TinyMcePage;

    [Url("controls/tinymce")]
    [VerifyTitle("TinyMCE")]
    public class TinyMcePage : Page<_>
    {
        public FrameWrappedContentEditor<_> EditorAsFrameWrappedContentEditor { get; private set; }
    }
}
