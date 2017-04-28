using _ = Atata.Tests.FrameInner2Page;

namespace Atata.Tests
{
    [VerifyH1]
    public class FrameInner2Page : Page<_>
    {
        public H1<_> Header { get; private set; }

        public Select<int, _> Select { get; private set; }

        public FramePage SwitchBack()
        {
            return SwitchToRoot<FramePage>();
        }
    }
}
