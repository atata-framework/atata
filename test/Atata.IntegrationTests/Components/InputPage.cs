﻿namespace Atata.IntegrationTests;

using _ = InputPage;

[Url("input")]
[VerifyTitle]
public class InputPage : Page<_>
{
    [TermSettings(TermCase.Lower)]
    public enum Option
    {
#pragma warning disable CA1712 // Do not prefix enum values with type name
        OptionA,
        OptionB,
        OptionC,
        OptionD
#pragma warning restore CA1712 // Do not prefix enum values with type name
    }

    [Term(CutEnding = false)]
    public TextInput<_> TextInput { get; private set; }

    [Term("Text Input")]
    [Format("!{0}!")]
    public TextInput<_> TextInputWithFormat { get; private set; }

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

    [FindById(CutEnding = false)]
    public FileInput<_> HiddenFileInput { get; private set; }

    [FindById(CutEnding = false)]
    public FileInput<_> TransparentFileInput { get; private set; }

    public SearchInput<_> SearchInput { get; private set; }

    [ScrollUp(TriggerEvents.BeforeAnyAction)]
    public RadioButton<_> SomeRadio { get; private set; }

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
            return ConvertStringToValueUsingGetFormat(valueAsString);
        }

        protected override void SetValue(DateTime? value)
        {
            string? valueAsString = ConvertValueToStringUsingSetFormat(value);
            AssociatedInput.Set(valueAsString!);
        }
    }
}
