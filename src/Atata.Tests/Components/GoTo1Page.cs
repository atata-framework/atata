using _ = Atata.Tests.GoTo1Page;

namespace Atata.Tests
{
    [Url("GoTo1.html")]
    [VerifyTitle("GoTo 1")]
    public class GoTo1Page : Page<_>
    {
        public LinkDelegate<GoTo2Page, _> GoTo2 { get; private set; }

        [GoTemporarily]
        public LinkDelegate<GoTo2Page, _> GoTo2Temporarily { get; private set; }

        public LinkDelegate<_> GoTo2Blank { get; private set; }
    }
}
