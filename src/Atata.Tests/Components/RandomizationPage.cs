using System;
using System.Linq;
using _ = Atata.Tests.RandomizationPage;

namespace Atata.Tests
{
    [Url("Randomization.html")]
    public class RandomizationPage : Page<_>
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
            OptionsDF = OptionD | OptionF,
            OptionsAE = OptionA | OptionE
        }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        public CheckBoxList<Options, _> SimpleEnum { get; private set; }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        [RandomizeCount(2, 4)]
        public CheckBoxList<Options, _> MultipleEnums { get; private set; }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        [RandomizeCount(2)]
        [RandomizeExclude(Options.None, Options.OptionB, Options.OptionD, Options.OptionF)]
        public CheckBoxList<Options, _> MultipleEnumsExcludingNoneBDF { get; private set; }

        [FindById("enum-checkboxes")]
        [FindItemByLabel]
        [RandomizeCount(3)]
        [RandomizeInclude(Options.OptionA, Options.OptionB, Options.OptionD, Options.OptionE, Options.OptionF)]
        public CheckBoxList<Options, _> MultipleEnumsIncludingABDEF { get; private set; }

        [FindById("enum-checkboxes")]
        public ItemsControl<CheckBox<_>, _> AllCheckBoxes { get; private set; }

        public DataProvider<int, _> CheckedItemsCount =>
            GetOrCreateDataProvider(
                nameof(CheckedItemsCount),
                () => AllCheckBoxes.Items.Where(x => x.IsChecked).Count());
    }
}
