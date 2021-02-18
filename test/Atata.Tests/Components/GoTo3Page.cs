using _ = Atata.Tests.GoTo3Page;

namespace Atata.Tests
{
    [Url("goto3")]
    [VerifyTitle("GoTo 3")]
    public class GoTo3Page : Page<_>
    {
        public LinkDelegate<GoTo1Page, _> GoTo1 { get; private set; }

        [GoTemporarily]
        public LinkDelegate<GoTo1Page, _> GoTo1Temporarily { get; private set; }

        public LinkDelegate<_> GoTo1Blank { get; private set; }
    }
}
