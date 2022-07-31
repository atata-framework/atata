namespace Atata.IntegrationTests
{
    using _ = GoTo1Page;

    [Url("goto1")]
    [VerifyTitle("GoTo 1")]
    public class GoTo1Page : Page<_>
    {
        public LinkDelegate<GoTo2Page, _> GoTo2 { get; private set; }

        [Term("Go to 2")]
        public Link<_> GoTo2Control { get; private set; }

        [GoTemporarily]
        public LinkDelegate<GoTo2Page, _> GoTo2Temporarily { get; private set; }

        public LinkDelegate<_> GoTo2Blank { get; private set; }
    }
}
