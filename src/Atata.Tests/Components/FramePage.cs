using _ = Atata.Tests.FramePage;

namespace Atata.Tests
{
    [Url("Frame.html")]
    [VerifyTitle]
    [VerifyH1]
    public class FramePage : Page<_>
    {
        public H1<_> Header { get; private set; }
    }
}
