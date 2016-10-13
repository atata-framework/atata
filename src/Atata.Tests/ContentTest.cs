using System;
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

        [Test]
        public void Date()
        {
            VerifyEquals(page.Date, new DateTime(2016, 5, 15));
            VerifyEquals(page.DateNull, null);
            page.DateNull.Content.Should.Equal(string.Empty);

            VerifyEquals(page.DateWithFormat, new DateTime(2016, 6, 15));
        }

        [Test]
        public void Time()
        {
            VerifyEquals(page.Time, new TimeSpan(17, 15, 0));
            VerifyEquals(page.TimeNull, null);
            page.TimeNull.Content.Should.Equal(string.Empty);

            VerifyEquals(page.TimeOfDay, new TimeSpan(14, 45, 0));
        }

        [Test]
        public void DateTime()
        {
            VerifyEquals(page.DateTime, new DateTime(2016, 5, 15, 13, 45, 0));

            page.DateTime.Should.EqualDate(new DateTime(2016, 5, 15)).
                DateTime.Should.BeGreater(new DateTime(2016, 5, 15)).
                DateTime.Should.BeLess(new DateTime(2016, 5, 16));

            VerifyEquals(page.DateTimeWithFormat, new DateTime(2009, 6, 15, 13, 45, 0));
        }
    }
}
