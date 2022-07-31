namespace Atata.IntegrationTests;

using _ = FrameInner2Page;

[VerifyH1]
public class FrameInner2Page : Page<_>
{
    public H1<_> Header { get; private set; }

    public Select<int, _> Select { get; private set; }

    public FramePage SwitchBack() =>
        SwitchToRoot<FramePage>();
}
