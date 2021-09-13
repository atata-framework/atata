using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class SelectTests : UITestFixture
    {
        private SelectPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<SelectPage>();
        }

        [Test]
        public void Select_String()
        {
            var control = _page.TextSelect;

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
            var control = _page.TextSelectWithFromat;

            SetAndVerifyValues(control, "A", "B");

            VerifyDoesNotEqual(control, "C");

            control.Options[1].Should.Equal("A");
            control.Options[4].Should.Equal("D");
        }

        [Test]
        public void Select_String_WithMatch()
        {
            var control = _page.TextSelectWithContainsMatch;

            control.Set("A");
            control.Should.Equal("Option A");

            control.Set("C");
            control.SelectedOption.Should.Equal("Option C");
        }

        [Test]
        public void Select_Enum_ByText()
        {
            SetAndVerifyValues(_page.EnumSelectByText, SelectPage.Option.OptionA, SelectPage.Option.OptionC);

            VerifyDoesNotEqual(_page.EnumSelectByText, SelectPage.Option.OptionD);
        }

        [Test]
        public void Select_Enum_ByValue()
        {
            SetAndVerifyValues(_page.EnumSelectByValue, SelectPage.Option.OptionB, SelectPage.Option.OptionA);

            VerifyDoesNotEqual(_page.EnumSelectByValue, SelectPage.Option.OptionB);
        }

        [Test]
        public void Select_Int_ByText()
        {
            VerifyEquals(_page.IntSelectByText, 1);

            SetAndVerifyValues(_page.IntSelectByText, 4, 2);

            VerifyDoesNotEqual(_page.IntSelectByText, 3);
        }

        [Test]
        public void Select_Enum_WithEmptyOption()
        {
            var sut = _page.EnumSelectWithEmptyOption;

            SetAndVerifyValues(sut, SelectPage.EnumWithEmptyOption.A, SelectPage.EnumWithEmptyOption.None);

            VerifyDoesNotEqual(sut, SelectPage.EnumWithEmptyOption.B);
        }

        [Test]
        public void Select_Enum_NullableWithoutEmptyOption()
        {
            var sut = _page.NullableEnumSelectWithEmptyOption;

            SetAndVerifyValues(sut, SelectPage.EnumWithoutEmptyOption.A, null, SelectPage.EnumWithoutEmptyOption.B);

            VerifyDoesNotEqual(sut, SelectPage.EnumWithoutEmptyOption.C);
        }
    }
}
