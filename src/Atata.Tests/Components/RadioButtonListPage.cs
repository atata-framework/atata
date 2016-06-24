using _ = Atata.Tests.RadioButtonListPage;

namespace Atata.Tests
{
    [NavigateTo("RadioButtonList.html")]
    [VerifyTitle(TermFormat.Pascal)]
    public class RadioButtonListPage : Page<_>
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
        public RadioButtonList<ByLabel?, _> ByNameAndLabel { get; private set; }

        [FindByClass("x-radio-container"), FindItemByValue]
        public RadioButtonList<ByValue, _> ByClassAndValue { get; private set; }

        [FindByCss(".x-radio-container"), FindItemByValue(TermFormat.Pascal)]
        public RadioButtonList<ByLabel, _> ByCssAndValue { get; private set; }

        [FindByClass(TermFormat.Snake), FindItemByLabel]
        public RadioButtonList<int?, _> IntegerItems { get; private set; }

        [FindByName, FindItemByLabel, Format("C")]
        public RadioButtonList<decimal?, _> DecimalItems { get; private set; }

        [FindByName(TermFormat.Kebab), FindItemByLabel]
        public RadioButtonList<string, _> VerticalItems { get; private set; }

        [FindByFieldSet("Vertical List"), FindItemByLabel]
        public RadioButtonList<string, _> VerticalItemsByFieldSet { get; private set; }
    }
}
