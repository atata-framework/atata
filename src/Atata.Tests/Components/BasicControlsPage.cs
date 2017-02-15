using _ = Atata.Tests.BasicControlsPage;

namespace Atata.Tests
{
    [Url("BasicControls.html")]
    [VerifyTitle]
    [VerifyH1]
    [VerifyContent("First Name", "Age")]
    public class BasicControlsPage : Page<_>
    {
        public H1<_> Header { get; private set; }

        public FindByLabel ByLabel { get; private set; }

        public FindById ById { get; private set; }

        [Term("Raw Button")]
        [FindByContent]
        public Button<_> RawButtonControl { get; private set; }

        [Term("Input Button")]
        public Button<_> InputButtonControl { get; private set; }

        [Term("Link", Format = "{0} Button")]
        public Link<_> LinkButtonControl { get; private set; }

        [Term("Div")]
        [FindByContent(Format = "{0} Button")]
        public Clickable<_> ClickableControl { get; private set; }

        [Term("Disabled Button")]
        public Button<_> DisabledButtonControl { get; private set; }

        [Term("Missing Button")]
        public Button<_> MissingButtonControl { get; private set; }

        [Term(CutEnding = false)]
        [FindByContent]
        public ButtonDelegate<_> RawButton { get; private set; }

        [Term(CutEnding = false)]
        public ButtonDelegate<_> InputButton { get; private set; }

        [Term(CutEnding = false)]
        public LinkDelegate<_> LinkButton { get; private set; }

        [Term(CutEnding = false)]
        [FindByContent]
        public ClickableDelegate<_> DivButton { get; private set; }

        [Term(CutEnding = false)]
        public ButtonDelegate<_> DisabledButton { get; private set; }

        [Term(CutEnding = false)]
        public ButtonDelegate<_> MissingButton { get; private set; }

        [Term(CutEnding = false)]
        [FindByContent]
        public ButtonDelegate<InputPage, _> GoToButton { get; private set; }

        [Term(CutEnding = false)]
        public ButtonDelegate<InputPage, _> GoToInputButton { get; private set; }

        [Term(CutEnding = false)]
        public LinkDelegate<InputPage, _> GoToLink { get; private set; }

        [Term(CutEnding = false)]
        [FindByContent]
        public ClickableDelegate<InputPage, _> GoToDivButton { get; private set; }

        public ControlList<TextInput<_>, _> AllTextInputs { get; private set; }

        [IgnoreInit]
        public TextInput<_> IgnoredInput { get; private set; }

        [ControlDefinition("*")]
        public class FindByLabel : Control<_>
        {
            public TextInput<_> FirstName { get; private set; }

            [Term(TermMatch.StartsWith)]
            public TextInput<_> LastName { get; private set; }

            [Term(TermCase.Sentence, Format = "{0}:")]
            public TextInput<_> MiddleName { get; private set; }

            public TextInput<_> ReadonlyField { get; private set; }

            public TextInput<_> DisabledField { get; private set; }
        }

        [ControlDefinition("*")]
        [ControlFinding(FindTermBy.Id, ControlType = typeof(Field<,>))]
        public class FindById : Control<_>
        {
            [FindById(TermCase.Kebab)]
            public TextInput<_> FirstName { get; private set; }

            [Term(TermCase.Camel)]
            public TextInput<_> LastName { get; private set; }

            [Term(TermCase.Pascal)]
            public TextInput<_> MiddleName { get; private set; }
        }
    }
}
