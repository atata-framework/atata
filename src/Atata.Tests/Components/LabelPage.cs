using _ = Atata.Tests.LabelPage;

namespace Atata.Tests
{
    [Url("label")]
    [VerifyTitle]
    [VerifyH1]
    public class LabelPage : Page<_>
    {
        public Label<_> FirstNameLabel { get; private set; }

        [Term(Format = "{0}*")]
        [Format("{0}*")]
        public Label<_> LastNameLabel { get; private set; }

        [FindByAttribute("for", "last-name")]
        public Label<_> LastNameByForLabel { get; private set; }
    }
}
