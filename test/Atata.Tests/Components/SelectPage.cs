using _ = Atata.Tests.SelectPage;

namespace Atata.Tests
{
    [Url("select")]
    [VerifyTitle]
    public class SelectPage : Page<_>
    {
        [TermSettings(TermCase.Title)]
        public enum Option
        {
#pragma warning disable CA1712 // Do not prefix enum values with type name
            [Term("--select--")]
            None,
            OptionA,
            OptionB,
            OptionC,
            OptionD
#pragma warning restore CA1712 // Do not prefix enum values with type name
        }

        [Term(CutEnding = false)]
        public Select<_> TextSelect { get; private set; }

        [Term("Text Select")]
        [Format("Option {0}")]
        public Select<_> TextSelectWithFromat { get; private set; }

        [Term("Text Select")]
        [SelectByText(TermMatch.Contains)]
        public Select<_> TextSelectWithContainsMatch { get; private set; }

        [Term("Text Select")]
        [SelectByText]
        public Select<Option, _> EnumSelectByText { get; private set; }

        [Term("Text Select")]
        [SelectByValue(TermCase.Pascal)]
        public Select<Option, _> EnumSelectByValue { get; private set; }

        [Term("Int Select")]
        [SelectByText(Format = "0{0}")]
        public Select<int, _> IntSelectByText { get; private set; }

        [Term("Int Select")]
        [SelectByValue]
        public Select<int, _> IntSelectByValue { get; private set; }
    }
}
