using _ = Atata.Tests.InputPage;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/Input.html")]
    public class InputPage : Page<_>
    {
        [TermSettings(TermFormat.LowerCase)]
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
        public TextInput<Option, _> EnumTextInput { get; private set; }

        [Term("Enum Text Input")]
        public TextInput<Option?, _> NullableEnumTextInput { get; private set; }

        [Term(CutEnding = false)]
        public TextInput<int?, _> IntTextInput { get; private set; }
    }
}
