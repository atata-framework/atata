using _ = Atata.Tests.ContentPage;

namespace Atata.Tests
{
    [NavigateTo("Content.html")]
    [VerifyTitle]
    [ControlFinding(FindTermBy.DescriptionTerm, ControlType = typeof(Content<,>))]
    public class ContentPage : Page<_>
    {
        public Text<_> Text { get; private set; }

        public Text<_> TextWithSpaces { get; private set; }

        [FindById]
        public Text<_> TextNull { get; private set; }

        public Number<_> Number { get; private set; }

        public Number<_> NumberZero { get; private set; }

        [FindById]
        public Number<_> NumberNull { get; private set; }

        [Format("count: {0}")]
        public Number<_> NumberWithFormat { get; private set; }
    }
}
