using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class RadioButtonListTests : UITestFixture
    {
        private RadioButtonListPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<RadioButtonListPage>();
        }

        [Test]
        public void RadioButtonList_Enum()
        {
            _page.ByNameAndLabel.Should.Equal(null).
                ByNameAndLabel.Should.BeNull();

            _page.ByClassAndValue.Should.Equal(RadioButtonListPage.ByValue.None);

            SetAndVerifyValues(
                _page.ByNameAndLabel,
                RadioButtonListPage.ByLabel.OptionC,
                RadioButtonListPage.ByLabel.OptionB);

            SetAndVerifyValues(
                _page.ByClassAndValue,
                RadioButtonListPage.ByValue.OptionD,
                RadioButtonListPage.ByValue.OptionA);

            SetAndVerifyValues(
                _page.ByCssAndValue,
                RadioButtonListPage.ByLabel.OptionB,
                RadioButtonListPage.ByLabel.OptionC);

            Assert.Throws<NoSuchElementException>(() =>
                _page.ByClassAndValue.Set(RadioButtonListPage.ByValue.MissingValue));

            Assert.Throws<ArgumentNullException>(() =>
                _page.ByNameAndLabel.Set(null));
        }

        [Test]
        public void RadioButtonList_String()
        {
            _page.VerticalItems.Should.Equal("Item 1");
            _page.VerticalItems.Should.Not.BeNull();

            SetAndVerifyValues(_page.VerticalItems, "Item 2", "Item 5");
            SetAndVerifyValues(_page.VerticalItemsByFieldSet, "Item 3", "Item 1");

            Assert.Throws<NoSuchElementException>(() =>
                _page.VerticalItems.Set("Item 999"));

            Assert.Throws<ArgumentNullException>(() =>
                _page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_Int()
        {
            _page.IntegerItems.Should.BeNull();

            SetAndVerifyValues(_page.IntegerItems, 2, 3);

            Assert.Throws<NoSuchElementException>(() =>
                _page.IntegerItems.Set(9));

            Assert.Throws<ArgumentNullException>(() =>
                _page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_Decimal()
        {
            _page.DecimalItems.Should.BeNull();

            SetAndVerifyValues(_page.DecimalItems, 1000, 2500);
            SetAndVerifyValues(_page.DecimalItems, 3210.50m, 4310.10m);

            Assert.Throws<NoSuchElementException>(() =>
                _page.DecimalItems.Set(918.76m));

            Assert.Throws<ArgumentNullException>(() =>
                _page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_Bool()
        {
            var control = _page.BoolItems;

            control.Should.BeNull();

            SetAndVerifyValues(control, false, true);
        }

        [Test]
        public void RadioButtonList_String_FindItemByParentContentAttribute()
        {
            VerifyRegularStringBasedRadioButtonList(_page.TextInParentItems);
        }

        [Test]
        public void RadioButtonList_String_FindItemByFollowingSiblingContentAttribute()
        {
            VerifyRegularStringBasedRadioButtonList(_page.TextInFollowingSiblingItems);
        }

        [Test]
        public void RadioButtonList_String_FindItemByPrecedingSiblingContentAttribute()
        {
            VerifyRegularStringBasedRadioButtonList(_page.TextInPrecedingSiblingItems);
        }

        private static void VerifyRegularStringBasedRadioButtonList(RadioButtonList<string, RadioButtonListPage> control)
        {
            control.Should.BeNull();

            SetAndVerifyValues(control, "Option B", "Option C");

            Assert.Throws<NoSuchElementException>(() =>
                control.Set("Option Z"));

            Assert.Throws<ArgumentNullException>(() =>
                control.Set(null));
        }
    }
}
