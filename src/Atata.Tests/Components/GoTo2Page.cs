using _ = Atata.Tests.GoTo2Page;

namespace Atata.Tests
{
    [Url("goto2")]
    [VerifyTitle("GoTo 2")]
    public class GoTo2Page : Page<_>
    {
        public LinkDelegate<GoTo1Page, _> GoTo1 { get; private set; }

        [GoTemporarily]
        public LinkDelegate<GoTo1Page, _> GoTo1Temporarily { get; private set; }

        public LinkDelegate<_> GoTo1Blank { get; private set; }

        public LinkDelegate<GoTo3Page, _> GoTo3 { get; private set; }

        [GoTemporarily]
        public LinkDelegate<GoTo3Page, _> GoTo3Temporarily { get; private set; }

        public LinkDelegate<_> GoTo3Blank { get; private set; }
    }
}
