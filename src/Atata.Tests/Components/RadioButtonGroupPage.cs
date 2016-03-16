using _ = Atata.Tests.RadioButtonGroupPage;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/RadioButtonGroup.html")]
    public class RadioButtonGroupPage : Page<_>
    {
        public enum ByLabel
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD
        }

        [TermSettings(TermFormat.Title)]
        public enum ByValue
        {
            None,
            [Term(TermFormat.Pascal)]
            OptionA,
            [Term(TermFormat.Pascal)]
            OptionB,
            [Term("COption", "OptionC")]
            OptionC,
            [Term(TermFormat.Pascal)]
            OptionD,
            MissingValue
        }

        [FindByName("radio-options"), FindItemByLabel]
        public RadioButtonGroup<ByLabel?, _> ByNameAndLabel { get; private set; }

        [FindByClass("x-radio-container"), FindItemByValue]
        public RadioButtonGroup<ByValue, _> ByClassAndValue { get; private set; }

        [FindByCss(".x-radio-container"), FindItemByValue(TermFormat.Pascal)]
        public RadioButtonGroup<ByLabel, _> ByCssAndValue { get; private set; }

        [FindByClass(TermFormat.Underscored), FindItemByLabel]
        public RadioButtonGroup<int?, _> IntegerItems { get; private set; }

        [FindByName(TermFormat.Dashed), FindItemByLabel]
        public RadioButtonGroup<string, _> VerticalItems { get; private set; }

        [FindByFieldset("Vertical List"), FindItemByLabel]
        public RadioButtonGroup<string, _> VerticalItemsByFieldset { get; private set; }
    }
}
