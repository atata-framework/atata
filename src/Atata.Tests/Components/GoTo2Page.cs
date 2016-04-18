using _ = Atata.Tests.GoTo2Page;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/GoTo2.html")]
    [VerifyTitle("GoTo 2")]
    public class GoTo2Page : Page<_>
    {
        public Link<GoTo1Page, _> GoTo1 { get; private set; }

        [GoTemporarily]
        public Link<GoTo1Page, _> GoTo1Temporarily { get; private set; }

        public Link<_> GoTo1Blank { get; private set; }

        [GoTemporarily]
        public Link<_> GoTo1BlankTemporarily { get; private set; }
    }
}
