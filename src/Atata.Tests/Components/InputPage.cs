using _ = Atata.Tests.InputPage;

namespace Atata.Tests
{
    [NavigateTo("Input.html")]
    [VerifyTitle]
    public class InputPage : Page<_>
    {
        [TermSettings(TermFormat.Lower)]
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
    }
}
