using _ = Atata.Tests.GoTo3Page;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/GoTo3.html")]
    [VerifyTitle("GoTo 3")]
    public class GoTo3Page : Page<_>
    {
        public Link<GoTo1Page, _> GoTo1 { get; private set; }

        [GoTemporarily]
        public Link<GoTo1Page, _> GoTo1Temporarily { get; private set; }

        public Link<_> GoTo1Blank { get; private set; }
    }
}
