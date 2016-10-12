using NUnit.Framework;

namespace Atata.Tests
{
    public class ContentTest : AutoTest
    {
        private ContentPage page;

        protected override void OnSetUp()
        {
            page = Go.To<ContentPage>();
        }

        [Test]
        public void Text()
        {
            VerifyEquals(page.Text, "Some Text");
            VerifyEquals(page.TextWithSpaces, "Some Text");
            VerifyEquals(page.TextNull, null);
            page.TextNull.Content.Should.Equal(string.Empty);
        }

        [Test]
        public void Number()
        {
            VerifyEquals(page.Number, 125.26m);
            VerifyEquals(page.NumberZero, 0);
            VerifyEquals(page.NumberNull, null);
            page.NumberNull.Content.Should.Equal(string.Empty);

            VerifyEquals(page.NumberWithFormat, 59);
            VerifyDoesNotEqual(page.NumberWithFormat, 55);
        }

        [Test]
        public void Currency()
        {
            VerifyEquals(page.Currency, 125234.26m);
            VerifyDoesNotEqual(page.Currency, 125234);
            VerifyEquals(page.CurrencyFR, -123.456m);
        }
    }
}
