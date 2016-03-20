using System;
using _ = Atata.Tests.CheckBoxListPage;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/CheckBoxList.html")]
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

        [FindById("enum-checkboxes"), FindItemByLabel]
        public CheckBoxList<Options, _> ByIdAndLabel { get; private set; }

        [FindByXPath("*[@id='enum-checkboxes']"), FindItemByValue(TermFormat.Pascal)]
        public CheckBoxList<Options, _> ByXPathAndValue { get; private set; }
    }
}
