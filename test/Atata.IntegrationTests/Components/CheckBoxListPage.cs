using System;

namespace Atata.IntegrationTests
{
    using _ = CheckBoxListPage;

    [Url("checkboxlist")]
    [VerifyTitle(TermCase.Pascal)]
    public class CheckBoxListPage : Page<_>
    {
        [Flags]
        public enum Options
        {
            None = 0,
            OptionA = 1 << 0,
            OptionB = 1 << 1,
            OptionC = 1 << 2,
            OptionD = 1 << 3,
            OptionE = 1 << 4,
            OptionF = 1 << 5,
            MissingValue = 1 << 6,
            OptionsDF = OptionD | OptionF
        }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        public CheckBoxList<Options, _> ByIdAndLabel { get; private set; }

        [FindByXPath("*[@id='enum-checkboxes']")]
        [FindItemByValue(TermCase.Pascal)]
        public CheckBoxList<Options, _> ByXPathAndValue { get; private set; }

        [FindByFieldSet("String List Using ID")]
        [FindItemByLabel]
        public CheckBoxList<Options, _> ByFieldsetAndLabelUsingId { get; private set; }

        public CheckBox<_> OptionA { get; private set; }

        public ControlList<CheckBox<_>, _> AllItems { get; private set; }
    }
}
