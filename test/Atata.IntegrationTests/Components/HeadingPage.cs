namespace Atata.IntegrationTests
{
    using _ = HeadingPage;

    [Url("heading")]
    [VerifyH1("Heading 1")]
    [VerifyH2("Heading 1.1", Index = 0)]
    [VerifyH2(TermMatch.Contains, "Heading 1_2", "Heading 1.2", Index = 1)]
    [VerifyH1("Heading_2", "Heading 2")]
    [VerifyH2("Heading 2.1")]
    [VerifyH3("Heading 2.1.1")]
    [VerifyH3(TermMatch.EndsWith, "2.1.2")]
    [VerifyH4("Heading 2.1.2.1")]
    [VerifyH5("Heading 2.1.2.1.1")]
    [VerifyH6("Heading 2.1.2.1.1.1")]
    [VerifyH2("Heading 2.2", Index = 3)]
    public class HeadingPage : Page<_>
    {
    }
}
