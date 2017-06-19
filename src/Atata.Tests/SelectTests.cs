using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class SelectTests : UITestFixture
    {
        private SelectPage page;

        protected override void OnSetUp()
        {
            page = Go.To<SelectPage>();
        }

        [Test]
        public void Select_String()
        {
            VerifyEquals(page.TextSelect, "--select--");

            SetAndVerifyValues(page.TextSelect, "Option A", "Option B");

            VerifyDoesNotEqual(page.TextSelect, "Option C");

            Assert.Throws<NoSuchElementException>(() =>
                page.TextSelect.Set("Missing Value"));
        }

        [Test]
        public void Select_String_Formatted()
        {
            SetAndVerifyValues(page.FormattedTextSelect, "A", "B");

            VerifyDoesNotEqual(page.FormattedTextSelect, "C");
        }

        [Test]
        public void Select_Enum_ByText()
        {
            SetAndVerifyValues(page.EnumSelectByText, SelectPage.Option.OptionA, SelectPage.Option.OptionC);

            VerifyDoesNotEqual(page.EnumSelectByText, SelectPage.Option.OptionD);
        }

        [Test]
        public void Select_Enum_ByValue()
        {
            SetAndVerifyValues(page.EnumSelectByValue, SelectPage.Option.OptionB, SelectPage.Option.OptionA);

            VerifyDoesNotEqual(page.EnumSelectByValue, SelectPage.Option.OptionB);
        }

        [Test]
        public void Select_Int_ByText()
        {
            VerifyEquals(page.IntSelectByText, 1);

            SetAndVerifyValues(page.IntSelectByText, 4, 2);

            VerifyDoesNotEqual(page.IntSelectByText, 3);
        }
    }
}
