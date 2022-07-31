using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.IntegrationTests
{
    public class CheckBoxListTests : UITestFixture
    {
        private CheckBoxListPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<CheckBoxListPage>();
        }

        [Test]
        public void CheckBoxList_Enum()
        {
            _page.ByIdAndLabel.Should.Equal(CheckBoxListPage.Options.None);

            SetAndVerifyValues(
                _page.ByIdAndLabel,
                CheckBoxListPage.Options.OptionC | CheckBoxListPage.Options.OptionD,
                CheckBoxListPage.Options.OptionB);

            SetAndVerifyValues(
                _page.ByXPathAndValue,
                CheckBoxListPage.Options.OptionA,
                CheckBoxListPage.Options.OptionsDF);

            _page.ByIdAndLabel.Should.Not.HaveChecked(CheckBoxListPage.Options.OptionA).
                ByIdAndLabel.Should.HaveChecked(CheckBoxListPage.Options.OptionD | CheckBoxListPage.Options.OptionF);

            SetAndVerifyValues(
                _page.ByXPathAndValue,
                CheckBoxListPage.Options.None,
                CheckBoxListPage.Options.OptionA);

            _page.ByIdAndLabel.Check(CheckBoxListPage.Options.OptionD);
            _page.ByXPathAndValue.Should.Equal(CheckBoxListPage.Options.OptionA | CheckBoxListPage.Options.OptionD);

            _page.ByXPathAndValue.Uncheck(CheckBoxListPage.Options.OptionA);
            _page.ByIdAndLabel.Should.HaveChecked(CheckBoxListPage.Options.OptionD);

            Assert.Throws<AssertionException>(() =>
                _page.ByIdAndLabel.Should.AtOnce.Not.HaveChecked(CheckBoxListPage.Options.OptionD));

            Assert.Throws<AssertionException>(() =>
                _page.ByIdAndLabel.Should.AtOnce.HaveChecked(CheckBoxListPage.Options.OptionA));

            Assert.Throws<NoSuchElementException>(() =>
                _page.ByIdAndLabel.Set(CheckBoxListPage.Options.MissingValue));
        }

        [Test]
        public void CheckBoxList_Enum_LabelById()
        {
            var control = _page.ByFieldsetAndLabelUsingId;

            control.Should.Equal(CheckBoxListPage.Options.None);

            SetAndVerifyValues(
                control,
                CheckBoxListPage.Options.OptionC | CheckBoxListPage.Options.OptionD,
                CheckBoxListPage.Options.OptionB);

            control.Should.Not.HaveChecked(CheckBoxListPage.Options.OptionA);
            control.Should.HaveChecked(CheckBoxListPage.Options.OptionB);

            SetAndVerifyValues(
                control,
                CheckBoxListPage.Options.None,
                CheckBoxListPage.Options.OptionA);

            control.Check(CheckBoxListPage.Options.OptionD);
            control.Should.Equal(CheckBoxListPage.Options.OptionA | CheckBoxListPage.Options.OptionD);

            control.Uncheck(CheckBoxListPage.Options.OptionA);
            control.Should.HaveChecked(CheckBoxListPage.Options.OptionD);

            Assert.Throws<AssertionException>(() =>
                control.Should.AtOnce.Not.HaveChecked(CheckBoxListPage.Options.OptionD));

            Assert.Throws<AssertionException>(() =>
                control.Should.AtOnce.HaveChecked(CheckBoxListPage.Options.OptionA));

            Assert.Throws<NoSuchElementException>(() =>
                control.Set(CheckBoxListPage.Options.MissingValue));
        }
    }
}
