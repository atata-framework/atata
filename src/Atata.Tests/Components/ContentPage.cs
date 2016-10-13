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

        public Currency<_> Currency { get; private set; }

        [Format("C3")]
        [Culture("fr-FR")]
        public Currency<_> CurrencyFR { get; private set; }

        public Date<_> Date { get; private set; }

        [FindById]
        public Date<_> DateNull { get; private set; }

        [Format("{0:dd, MMMM yyyy} year")]
        public Date<_> DateWithFormat { get; private set; }

        public Time<_> Time { get; private set; }

        [FindById]
        public Time<_> TimeNull { get; private set; }

        [Format("h:mm tt")]
        public Time<_> TimeOfDay { get; private set; }
    }
}
