using _ = Atata.Tests.BasicControlsPage;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/BasicControls.html")]
    [VerifyTitle("Basic Controls")]
    [VerifyContentContainsAll("First Name", "Age")]
    public class BasicControlsPage : Page<BasicControlsPage>
    {
        [FindByCss("h1")]
        public Text<_> Header { get; private set; }

        public FindByLabel ByLabel { get; private set; }

        public FindById ById { get; private set; }

        [Term("Raw Button")]
        [FindByContent]
        public ButtonControl<_> RawButtonControl { get; private set; }

        [Term("Input Button")]
        public ButtonControl<_> InputButtonControl { get; private set; }

        [Term("Link Button")]
        public LinkControl<_> LinkButtonControl { get; private set; }

        [Term("Div Button")]
        public ClickableControl<_> ClickableControl { get; private set; }

        [Term("Disabled Button")]
        public ButtonControl<_> DisabledButtonControl { get; private set; }

        [Term("Missing Button")]
        public ButtonControl<_> MissingButtonControl { get; private set; }

        [Term(CutEnding = false)]
        [FindByContent]
        public Button<_> RawButton { get; private set; }

        [Term(CutEnding = false)]
        public Button<_> InputButton { get; private set; }

        [Term(CutEnding = false)]
        public Link<_> LinkButton { get; private set; }

        [Term(CutEnding = false)]
        public Clickable<_> DivButton { get; private set; }

        [Term(CutEnding = false)]
        public Button<_> DisabledButton { get; private set; }

        [Term(CutEnding = false)]
        public Button<_> MissingButton { get; private set; }

        [Term(CutEnding = false)]
        [FindByContent]
        public Button<InputPage, _> GoToButton { get; private set; }

        [Term(CutEnding = false)]
        public Button<InputPage, _> GoToInputButton { get; private set; }

        [Term(CutEnding = false)]
        public Link<InputPage, _> GoToLink { get; private set; }

        [Term(CutEnding = false)]
        public Clickable<InputPage, _> GoToDivButton { get; private set; }

        [UIComponent("*")]
        public class FindByLabel : Control<_>
        {
            public TextInput<_> FirstName { get; private set; }

            [Term(TermMatch.StartsWith)]
            public TextInput<_> LastName { get; private set; }

            [Term(TermFormat.SentenceWithColon)]
            public TextInput<_> MiddleName { get; private set; }

            public TextInput<_> ReadonlyField { get; private set; }

            public TextInput<_> DisabledField { get; private set; }
        }

        [UIComponent("*")]
        [FindFields(FindTermBy.Id)]
        public class FindById : Control<_>
        {
            [FindById(TermFormat.Dashed)]
            public TextInput<_> FirstName { get; private set; }

            [Term(TermFormat.Camel)]
            public TextInput<_> LastName { get; private set; }

            [Term(TermFormat.Pascal)]
            public TextInput<_> MiddleName { get; private set; }
        }
    }
}
