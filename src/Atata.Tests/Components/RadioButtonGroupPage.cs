using _ = Atata.Tests.RadioButtonGroupPage;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/RadioButtonGroup.html")]
    public class RadioButtonGroupPage : Page<_>
    {
        public enum RadioOptionLabel
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD
        }

        [TermSettings(TermFormat.Title)]
        public enum RadioOptionValue
        {
            [Term(TermFormat.Pascal)]
            OptionA,
            [Term(TermFormat.Pascal)]
            OptionB,
            [Term(TermFormat.Pascal)]
            OptionC,
            [Term(TermFormat.Pascal)]
            OptionD,
            MissingValue
        }

        [FindByName("radio-options"), FindItemByLabel]
        public RadioButtonGroup<RadioOptionLabel, _> RadioOptionsByNameAndLabel { get; private set; }

        [FindByClass("x-radio-container"), FindItemByValue]
        public RadioButtonGroup<RadioOptionValue, _> RadioOptionsByClassAndValue { get; private set; }

        [FindByCss(".x-radio-container"), FindItemByValue]
        public RadioButtonGroup<RadioOptionValue, _> RadioOptionsByCssAndValue { get; private set; }

        [FindByName(TermFormat.Dashed), FindItemByLabel]
        public RadioButtonGroup<string, _> VerticalItems { get; private set; }
    }
}
