using System;
using _ = Atata.Tests.InputPage;

namespace Atata.Tests
{
    [Url("input")]
    [VerifyTitle]
    public class InputPage : Page<_>
    {
        [TermSettings(TermCase.Lower)]
        public enum Option
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD
        }

        [Term(CutEnding = false)]
        public TextInput<_> TextInput { get; private set; }

        [Term(CutEnding = false)]
        public Input<Option, _> EnumTextInput { get; private set; }

        [Term("Enum Text Input")]
        public Input<Option?, _> NullableEnumTextInput { get; private set; }

        [Term(CutEnding = false)]
        public Input<int?, _> IntTextInput { get; private set; }

        [Term("Int Text Input")]
        public NumberInput<_> NumberInput { get; private set; }

        public TelInput<_> TelInput { get; private set; }

        public FileInput<_> FileInput { get; private set; }

        public SearchInput<_> SearchInput { get; private set; }

        public EmailInput<_> EmailInput { get; private set; }

        [Term("URL Input")]
        public UrlInput<_> UrlInput { get; private set; }

        [FindFirst]
        public CustomDatePicker DatePicker { get; private set; }

        [ControlDefinition(ContainingClass = "custom-datepicker")]
        [Format("d")]
        public class CustomDatePicker : EditableField<DateTime?, _>
        {
            [FindFirst]
            [TraceLog]
            [Name("Associated")]
            private TextInput<_> AssociatedInput { get; set; }

            protected override DateTime? GetValue()
            {
                string valueAsString = AssociatedInput.Value;
                return ConvertStringToValue(valueAsString);
            }

            protected override void SetValue(DateTime? value)
            {
                string valueAsString = ConvertValueToString(value);
                AssociatedInput.Set(valueAsString);
            }
        }
    }
}
