namespace Atata.Tests
{
    using _ = FrameInner1Page;

    [VerifyH1]
    public class FrameInner1Page : Page<_>
    {
        public H1<_> Header { get; private set; }

        public TextInput<_> TextInput { get; private set; }
    }
}
