namespace Atata.Tests
{
    using _ = SelectPage;

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

        public enum EnumWithEmptyOption
        {
            [Term("")]
            None,
            A,
            B,
            C
        }

        public enum EnumWithoutEmptyOption
        {
            A,
            B,
            C
        }

        [Term(CutEnding = false)]
        public Select<_> TextSelect { get; private set; }

        [Term("Text Select")]
        [Format("Option {0}")]
        public Select<_> TextSelectWithFromat { get; private set; }

        [Term("Text Select")]
        [SelectsOptionByText(TermMatch.Contains)]
        public Select<_> TextSelectWithContainsMatch { get; private set; }

        [Term("Text Select")]
        [SelectsOptionByText]
        public Select<Option, _> EnumSelectByText { get; private set; }

        [Term("Text Select")]
        [SelectsOptionByValue(TermCase.Pascal)]
        public Select<Option, _> EnumSelectByValue { get; private set; }

        [Term("Int Select")]
        [SelectsOptionByText(Format = "0{0}")]
        public Select<int, _> IntSelectByText { get; private set; }

        [Term("Int Select")]
        [SelectsOptionByValue]
        public Select<int, _> IntSelectByValue { get; private set; }

        [Term("Text Select With Empty Option")]
        public Select<EnumWithEmptyOption, _> EnumSelectWithEmptyOption { get; private set; }

        [Term("Text Select With Empty Option")]
        public Select<EnumWithoutEmptyOption?, _> NullableEnumSelectWithEmptyOption { get; private set; }
    }
}
