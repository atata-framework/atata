using System;
using System.Linq;
using _ = Atata.Tests.RandomizationPage;

namespace Atata.Tests
{
    [Url("Randomization.html")]
    public class RandomizationPage : Page<_>
    {
        [Flags]
        public enum CheckBoxOptions
        {
            None = 0,
            OptionA = 1 << 0,
            OptionB = 1 << 1,
            OptionC = 1 << 2,
            OptionD = 1 << 3,
            OptionE = 1 << 4,
            OptionF = 1 << 5,
            OptionsDF = OptionD | OptionF,
            OptionsAE = OptionA | OptionE
        }

        [TermSettings(TermCase.Title)]
        public enum SelectOption
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD
        }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        public CheckBoxList<CheckBoxOptions, _> SimpleEnum { get; private set; }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        [RandomizeCount(2, 4)]
        public CheckBoxList<CheckBoxOptions, _> MultipleEnums { get; private set; }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        [RandomizeCount(2)]
        [RandomizeExclude(CheckBoxOptions.None, CheckBoxOptions.OptionB, CheckBoxOptions.OptionD, CheckBoxOptions.OptionF)]
        public CheckBoxList<CheckBoxOptions, _> MultipleEnumsExcludingNoneBDF { get; private set; }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        [RandomizeCount(3)]
        [RandomizeInclude(CheckBoxOptions.OptionA, CheckBoxOptions.OptionB, CheckBoxOptions.OptionD, CheckBoxOptions.OptionE, CheckBoxOptions.OptionF)]
        public CheckBoxList<CheckBoxOptions, _> MultipleEnumsIncludingABDEF { get; private set; }

        public CheckBox<_> OptionA { get; private set; }

        [FindById("enum-checkboxes")]
        public ItemsControl<CheckBox<_>, _> AllCheckBoxes { get; private set; }

        public DataProvider<int, _> CheckedItemsCount =>
            GetOrCreateDataProvider(
                nameof(CheckedItemsCount),
                () => AllCheckBoxes.Items.Where(x => x.IsChecked).Count());

        [FindById("text-select")]
        [RandomizeInclude("Option A", "Option B", "Option C")]
        public Select<_> TextSelect { get; private set; }

        [FindById("text-select")]
        public Select<SelectOption?, _> EnumSelect { get; private set; }

        [FindById("int-select")]
        [RandomizeNumberSettings(1, 2)]
        public Select<int?, _> IntSelect { get; private set; }

        [FindById("int-select")]
        [RandomizeInclude(1, 2, 3)]
        public Select<int?, _> IntSelectUsingInclude { get; private set; }
    }
}
