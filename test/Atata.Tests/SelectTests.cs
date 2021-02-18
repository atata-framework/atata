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
            var control = page.TextSelect;

            VerifyEquals(control, "--select--");
            control.SelectedIndex.Should.Equal(0);

            SetAndVerifyValues(control, "Option A", "Option B");
            control.SelectedIndex.Should.Equal(2);
            control.SelectedOption.Should.Equal("Option B");
            control.Options[2].IsSelected.Should.BeTrue();
            control.Options[0].IsSelected.Should.BeFalse();

            VerifyDoesNotEqual(control, "Option C");

            Assert.Throws<NoSuchElementException>(() =>
                control.Set("Missing Value"));

            control.Options.Should.EqualSequence("--select--", "Option A", "Option B", "Option C", "Option D");
        }

        [Test]
        public void Select_String_WithFormat()
        {
            var control = page.TextSelectWithFromat;

            SetAndVerifyValues(control, "A", "B");

            VerifyDoesNotEqual(control, "C");

            control.Options[1].Should.Equal("A");
            control.Options[4].Should.Equal("D");
        }

        [Test]
        public void Select_String_WithMatch()
        {
            var control = page.TextSelectWithContainsMatch;

            control.Set("A");
            control.Should.Equal("Option A");

            control.Set("C");
            control.SelectedOption.Should.Equal("Option C");
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
