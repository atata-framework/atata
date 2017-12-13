using _ = Atata.Tests.InputPage;

namespace Atata.Tests
{
    [Url("Input.html")]
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
    }
}
