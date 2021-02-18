using _ = Atata.Tests.FrameInner1Page;

namespace Atata.Tests
{
    [VerifyH1]
    public class FrameInner1Page : Page<_>
    {
        public H1<_> Header { get; private set; }

        public TextInput<_> TextInput { get; private set; }
    }
}
