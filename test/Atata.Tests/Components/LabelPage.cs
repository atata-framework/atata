using _ = Atata.Tests.LabelPage;

namespace Atata.Tests
{
    [Url("label")]
    [VerifyTitle]
    [VerifyH1]
    public class LabelPage : Page<_>
    {
        public Label<_> FirstNameLabel { get; private set; }

        [FindByLabel]
        public TextInput<_> FirstName { get; private set; }

        [Term(Format = "{0}*")]
        [Format("{0}*")]
        public Label<_> LastNameLabel { get; private set; }

        [FindById]
        public TextInput<_> LastName { get; private set; }

        [FindByAttribute("for", "last-name")]
        public Label<_> LastNameByForLabel { get; private set; }

        [FindById]
        public TextInput<_> CompanyName { get; private set; }

        public LabelList<_> Labels { get; private set; }
    }
}
