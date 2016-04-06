using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace Atata.Tests
{
    [TestFixture]
    public class RadioButtonListTest : TestBase
    {
        private RadioButtonListPage page;

        protected override void OnSetUp()
        {
            page = GoTo<RadioButtonListPage>();
        }

        [Test]
        public void RadioButtonList_Enum()
        {
            page.ByNameAndLabel.VerifyEquals(null);
            page.ByClassAndValue.VerifyEquals(RadioButtonListPage.ByValue.None);

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
            page.VerticalItems.VerifyEquals("Item 1");
            page.VerticalItems.VerifyDoesNotEqual(null);

            SetAndVerifyValues(page.VerticalItems, "Item 2", "Item 5");
            SetAndVerifyValues(page.VerticalItemsByFieldset, "Item 3", "Item 1");

            Assert.Throws<NoSuchElementException>(() =>
                page.VerticalItems.Set("Item 999"));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_Int()
        {
            page.IntegerItems.VerifyEquals(null);

            SetAndVerifyValues(page.IntegerItems, 2, 3);

            Assert.Throws<NoSuchElementException>(() =>
                page.IntegerItems.Set(9));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }

        [Test]
        public void RadioButtonList_Decimal()
        {
            page.DecimalItems.VerifyEquals(null);

            SetAndVerifyValues(page.DecimalItems, 1000, 2500);
            SetAndVerifyValues(page.DecimalItems, 3210.50m, 4310.10m);

            Assert.Throws<NoSuchElementException>(() =>
                page.DecimalItems.Set(918.76m));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }
    }
}
