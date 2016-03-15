using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace Atata.Tests
{
    [TestFixture]
    public class RadioButtonGroupTest : TestBase
    {
        private RadioButtonGroupPage page;

        protected override void OnSetUp()
        {
            page = GoTo<RadioButtonGroupPage>();
        }

        [Test]
        public void RadioButtonGroup_Enum()
        {
            page.ByNameAndLabel.VerifyEquals(null);
            page.ByClassAndValue.VerifyEquals(RadioButtonGroupPage.ByValue.None);

            TestRadioButtonGroup(
                page.ByNameAndLabel,
                RadioButtonGroupPage.ByLabel.OptionC,
                RadioButtonGroupPage.ByLabel.OptionB);

            TestRadioButtonGroup(
                page.ByClassAndValue,
                RadioButtonGroupPage.ByValue.OptionD,
                RadioButtonGroupPage.ByValue.OptionA);

            TestRadioButtonGroup(
                page.ByCssAndValue,
                RadioButtonGroupPage.ByLabel.OptionB,
                RadioButtonGroupPage.ByLabel.OptionC);

            Assert.Throws<NoSuchElementException>(() =>
                page.ByClassAndValue.Set(RadioButtonGroupPage.ByValue.MissingValue));

            Assert.Throws<ArgumentNullException>(() =>
                page.ByNameAndLabel.Set(null));
        }

        [Test]
        public void RadioButtonGroup_String()
        {
            page.VerticalItems.VerifyEquals("Item 1");
            page.VerticalItems.VerifyNotEqual(null);

            TestRadioButtonGroup(page.VerticalItems, "Item 2", "Item 5");
            TestRadioButtonGroup(page.VerticalItemsByFieldset, "Item 3", "Item 1");

            Assert.Throws<NoSuchElementException>(() =>
                page.VerticalItems.Set("Item 999"));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }

        private void TestRadioButtonGroup<T>(RadioButtonGroup<T, RadioButtonGroupPage> group, T value1, T value2)
        {
            group.VerifyExists();
            group.Set(value1);
            group.VerifyEquals(value1);
            Assert.That(group.Get(), Is.EqualTo(value1));

            group.Set(value2);
            group.VerifyNotEqual(value1);
            group.VerifyEquals(value2);
            Assert.That(group.Get(), Is.EqualTo(value2));
        }
    }
}
