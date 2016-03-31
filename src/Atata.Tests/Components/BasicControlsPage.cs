using _ = Atata.Tests.BasicControlsPage;

namespace Atata.Tests
{
    [VerifyTitle("Basic Controls")]
    [VerifyContentContainsAll("First Name", "Age")]
    [NavigateTo("http://localhost:50549/BasicControls.html")]
    public class BasicControlsPage : Page<BasicControlsPage>
    {
        [FindByCss("h1")]
        public Text<_> Header { get; private set; }

        public FindByLabel ByLabel { get; private set; }

        public FindById ById { get; private set; }

        [Term(CutEnding = false)]
        [FindByContent]
        public Button<_> RawButton { get; private set; }

        [Term(CutEnding = false)]
        public Button<_> InputButton { get; private set; }

        [Term(CutEnding = false)]
        public Link<_> LinkButton { get; private set; }

        [Term(CutEnding = false)]
        public Button<_> DisabledButton { get; private set; }

        [Term(CutEnding = false)]
        public Button<_> MissingButton { get; private set; }

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
