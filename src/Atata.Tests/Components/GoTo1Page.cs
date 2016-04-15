using _ = Atata.Tests.GoTo1Page;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/GoTo1.html")]
    [VerifyTitle("GoTo 1")]
    public class GoTo1Page : Page<_>
    {
        public Link<_> GoTo2 { get; private set; }
        public Link<_> GoTo2Blank { get; private set; }
    }
}
