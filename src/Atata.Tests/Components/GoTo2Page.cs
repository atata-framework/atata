using _ = Atata.Tests.GoTo2Page;

namespace Atata.Tests
{
    [Url("GoTo2.html")]
    [VerifyTitle("GoTo 2")]
    public class GoTo2Page : Page<_>
    {
        public Link<GoTo1Page, _> GoTo1 { get; private set; }

        [GoTemporarily]
        public Link<GoTo1Page, _> GoTo1Temporarily { get; private set; }

        public Link<_> GoTo1Blank { get; private set; }

        public Link<GoTo3Page, _> GoTo3 { get; private set; }

        [GoTemporarily]
        public Link<GoTo3Page, _> GoTo3Temporarily { get; private set; }

        public Link<_> GoTo3Blank { get; private set; }
    }
}
