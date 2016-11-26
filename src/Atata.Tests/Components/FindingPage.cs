using _ = Atata.Tests.FindingPage;

namespace Atata.Tests
{
    [Url("Finding.html")]
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

        [FindByXPath("[@name='radio-options']", Index = 2)]
        public RadioButton<_> OptionCByXPathCondition { get; private set; }

        [FindByXPath("@name='radio-options'", Index = 2)]
        public RadioButton<_> OptionCByXPathAttribute { get; private set; }

        [FindByAttribute("name", "radio-options", Index = 2)]
        public RadioButton<_> OptionCByAttribute { get; private set; }

        [FindByClass("radio-options", Index = 2)]
        public RadioButton<_> OptionCByClass { get; private set; }

        [FindLast]
        public RadioButton<_> OptionDAsLast { get; private set; }

        [FindById]
        public TextInput<_> VisibleInput { get; private set; }

        [FindById(Visibility = Visibility.Hidden)]
        public TextInput<_> DisplayNoneInput { get; private set; }

        [FindById("display-none-input")]
        public TextInput<_> FailDisplayNoneInput { get; private set; }

        [FindById(Visibility = Visibility.Hidden)]
        public TextInput<_> HiddenInput { get; private set; }

        [FindById(Visibility = Visibility.Hidden)]
        public TextInput<_> CollapseInput { get; private set; }

        [FindById]
        public HiddenInput<_> TypeHiddenInput { get; private set; }
    }
}
