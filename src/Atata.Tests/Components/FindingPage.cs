using _ = Atata.Tests.FindingPage;

namespace Atata.Tests
{
    [NavigateTo("Finding.html")]
    [VerifyTitle]
    [VerifyH1]
    public class FindingPage : Page<_>
    {
        [FindByIndex(2)]
        public RadioButton<_> OptionCByIndex { get; private set; }

        [FindByName("radio-options", Index = 2)]
        public RadioButton<_> OptionCByName { get; private set; }

        [FindByCss("[name='radio-options']", Index = 2)]
        public RadioButton<_> OptionCByCss { get; private set; }

        [FindByXPath("non-existent", "*[@name='radio-options']", Index = 2)]
        public RadioButton<_> OptionCByXPath { get; private set; }

        [FindByAttribute("name", "radio-options", Index = 2)]
        public RadioButton<_> OptionCByAttribute { get; private set; }

        [FindByClass("radio-options", Index = 2)]
        public RadioButton<_> OptionCByClass { get; private set; }
    }
}
