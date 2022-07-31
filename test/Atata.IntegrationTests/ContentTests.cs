using System;
using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class ContentTests : UITestFixture
    {
        private ContentPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<ContentPage>();
        }

        [Test]
        public void Text()
        {
            VerifyEquals(_page.Text, "Some Text");
            VerifyEquals(_page.TextWithSpaces, "Some Text");
            VerifyEquals(_page.TextNull, string.Empty);
            _page.TextNull.Content.Should.Equal(string.Empty);
        }

        [Test]
        public void Number()
        {
            VerifyEquals(_page.Number, 125.26m);
            VerifyEquals(_page.NumberZero, 0);
            VerifyEquals(_page.NumberNull, null);
            _page.NumberNull.Content.Should.Equal(string.Empty);

            VerifyEquals(_page.NumberWithFormat, 59);
            VerifyDoesNotEqual(_page.NumberWithFormat, 55);
        }

        [Test]
        public void Currency()
        {
            VerifyEquals(_page.Currency, 125234.26m);
            VerifyDoesNotEqual(_page.Currency, 125234);
            VerifyEquals(_page.CurrencyFR, -123.456m);
        }

        [Test]
        public void Date()
        {
            VerifyEquals(_page.Date, new DateTime(2016, 5, 15));
            VerifyEquals(_page.DateNull, null);
            _page.DateNull.Content.Should.Equal(string.Empty);

            VerifyEquals(_page.DateWithFormat, new DateTime(2016, 6, 15));
        }

        [Test]
        public void Time()
        {
            VerifyEquals(_page.Time, new TimeSpan(17, 15, 0));
            VerifyEquals(_page.TimeNull, null);
            _page.TimeNull.Content.Should.Equal(string.Empty);

            VerifyEquals(_page.TimeOfDay, new TimeSpan(14, 45, 0));
        }

        [Test]
        public void DateTime()
        {
            VerifyEquals(_page.DateTime, new DateTime(2016, 5, 15, 13, 45, 0));

            _page.DateTime.Should.EqualDate(new DateTime(2016, 5, 15)).
                DateTime.Should.BeGreater(new DateTime(2016, 5, 15)).
                DateTime.Should.BeLess(new DateTime(2016, 5, 16));

            VerifyEquals(_page.DateTimeWithFormat, new DateTime(2009, 6, 15, 13, 45, 0));
        }
    }
}
