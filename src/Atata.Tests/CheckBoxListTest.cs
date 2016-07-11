using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    [TestFixture]
    public class CheckBoxListTest : TestBase
    {
        private CheckBoxListPage page;

        protected override void OnSetUp()
        {
            page = Go.To<CheckBoxListPage>();
        }

        [Test]
        public void CheckBoxList_Enum()
        {
            page.ByIdAndLabel.Should.Equal(CheckBoxListPage.Options.None);

            SetAndVerifyValues(
                page.ByIdAndLabel,
                CheckBoxListPage.Options.OptionC | CheckBoxListPage.Options.OptionD,
                CheckBoxListPage.Options.OptionB);

            SetAndVerifyValues(
                page.ByXPathAndValue,
                CheckBoxListPage.Options.OptionA,
                CheckBoxListPage.Options.OptionsDF);

            page.ByIdAndLabel.VerifyUnchecked(CheckBoxListPage.Options.OptionA).
                ByIdAndLabel.VerifyChecked(CheckBoxListPage.Options.OptionD | CheckBoxListPage.Options.OptionF);

            SetAndVerifyValues(
                page.ByXPathAndValue,
                CheckBoxListPage.Options.None,
                CheckBoxListPage.Options.OptionA);

            page.ByIdAndLabel.Check(CheckBoxListPage.Options.OptionD);
            page.ByXPathAndValue.Should.Equal(CheckBoxListPage.Options.OptionA | CheckBoxListPage.Options.OptionD);

            page.ByXPathAndValue.Uncheck(CheckBoxListPage.Options.OptionA);
            page.ByIdAndLabel.VerifyChecked(CheckBoxListPage.Options.OptionD);

            Assert.Throws<AssertionException>(() =>
                page.ByIdAndLabel.VerifyUnchecked(CheckBoxListPage.Options.OptionD));

            Assert.Throws<AssertionException>(() =>
                page.ByIdAndLabel.VerifyChecked(CheckBoxListPage.Options.OptionA));

            Assert.Throws<NoSuchElementException>(() =>
                page.ByIdAndLabel.Set(CheckBoxListPage.Options.MissingValue));
        }
    }
}
