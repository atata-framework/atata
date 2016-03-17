using _ = Atata.Tests.CheckBoxListPage;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/CheckBoxList.html")]
    public class CheckBoxListPage : Page<_>
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

        [FindByClass("x-radio-container"), FindItemByValue]
        public CheckBoxList<ByLabel?, _> ByClassAndLabel { get; private set; }

        [FindByCss(".x-radio-container"), FindItemByValue(TermFormat.Pascal)]
        public CheckBoxList<ByLabel, _> ByCssAndValue { get; private set; }

        [FindByClass(TermFormat.Underscored), FindItemByLabel]
        public CheckBoxList<int?, _> IntegerItems { get; private set; }

        [FindByName, FindItemByLabel, Format("C")]
        public CheckBoxList<decimal?, _> DecimalItems { get; private set; }

        [FindByName(TermFormat.Dashed), FindItemByLabel]
        public CheckBoxList<string, _> VerticalItems { get; private set; }

        [FindByFieldset("Vertical List"), FindItemByLabel]
        public CheckBoxList<string, _> VerticalItemsByFieldset { get; private set; }
    }
}
