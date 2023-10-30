namespace Atata.IntegrationTests;

using _ = GoTo3Page;

[Url(DefaultUrl)]
public class GoTo3Page : Page<_>
{
    public const string DefaultUrl = "/goto3";

    public LinkDelegate<GoTo1Page, _> GoTo1 { get; private set; }

    [GoTemporarily]
    public LinkDelegate<GoTo1Page, _> GoTo1Temporarily { get; private set; }

    public LinkDelegate<_> GoTo1Blank { get; private set; }
}
