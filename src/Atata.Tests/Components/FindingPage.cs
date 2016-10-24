using _ = Atata.Tests.FindingPage;

namespace Atata.Tests
{
    [NavigateTo("Finding.html")]
    [VerifyTitle]
    [VerifyH1]
    public class FindingPage : Page<_>
    {
        [FindByName("radio-options", Index = 2)]
        public RadioButton<_> OptionC { get; private set; }
    }
}
