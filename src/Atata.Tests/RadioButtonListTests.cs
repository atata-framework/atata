using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class RadioButtonListTests : UITestFixture
    {
        private RadioButtonListPage page;

        protected override void OnSetUp()
        {
            page = Go.To<RadioButtonListPage>();
        }

        [Test]
        public void RadioButtonList_Enum()
        {
            page.ByNameAndLabel.Should.Equal(null).
                ByNameAndLabel.Should.BeNull();

            page.ByClassAndValue.Should.Equal(RadioButtonListPage.ByValue.None);

            SetAndVerifyValues(
                page.ByNameAndLabel,
                RadioButtonListPage.ByLabel.OptionC,
                RadioButtonListPage.ByLabel.OptionB);

            SetAndVerifyValues(
                page.ByClassAndValue,
                RadioButtonListPage.ByValue.OptionD,
                RadioButtonListPage.ByValue.OptionA);

            SetAndVerifyValues(
                page.ByCssAndValue,
                RadioButtonListPage.ByLabel.OptionB,
                RadioButtonListPage.ByLabel.OptionC);

            Assert.Throws<NoSuchElementException>(() =>
                page.ByClassAndValue.Set(RadioButtonListPage.ByValue.MissingValue));

            Assert.Throws<ArgumentNullException>(() =>
                page.ByNameAndLabel.Set(null));
        }

        [Test]
        public void RadioButtonList_String()
        {
            page.VerticalItems.Should.Equal("Item 1");
            page.VerticalItems.Should.Not.BeNull();

            SetAndVerifyValues(page.VerticalItems, "Item 2", "Item 5");
            SetAndVerifyValues(page.VerticalItemsByFieldSet, "Item 3", "Item 1");

            Assert.Throws<NoSuchElementException>(() =>
                page.VerticalItems.Set("Item 999"));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_Int()
        {
            page.IntegerItems.Should.BeNull();

            SetAndVerifyValues(page.IntegerItems, 2, 3);

            Assert.Throws<NoSuchElementException>(() =>
                page.IntegerItems.Set(9));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_Decimal()
        {
            page.DecimalItems.Should.BeNull();

            SetAndVerifyValues(page.DecimalItems, 1000, 2500);
            SetAndVerifyValues(page.DecimalItems, 3210.50m, 4310.10m);

            Assert.Throws<NoSuchElementException>(() =>
                page.DecimalItems.Set(918.76m));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_String_FindItemByParentContentAttribute()
        {
            VerifyRegularStringBasedRadioButtonList(page.TextInParentItems);
        }

        [Test]
        public void RadioButtonList_String_FindItemByFollowingSiblingContentAttribute()
        {
            VerifyRegularStringBasedRadioButtonList(page.TextInFollowingSiblingItems);
        }

        [Test]
        public void RadioButtonList_String_FindItemByPrecedingSiblingContentAttribute()
        {
            VerifyRegularStringBasedRadioButtonList(page.TextInPrecedingSiblingItems);
        }

        private void VerifyRegularStringBasedRadioButtonList(RadioButtonList<string, RadioButtonListPage> control)
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
