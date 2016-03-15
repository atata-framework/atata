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
            page.RadioOptionsByNameAndLabel.VerifyEquals(null);
            page.RadioOptionsByClassAndValue.VerifyEquals(RadioButtonGroupPage.RadioOptionValue.None);

            TestRadioButtonGroup(
                page.RadioOptionsByNameAndLabel,
                RadioButtonGroupPage.RadioOptionLabel.OptionC,
                RadioButtonGroupPage.RadioOptionLabel.OptionB);

            TestRadioButtonGroup(
                page.RadioOptionsByClassAndValue,
                RadioButtonGroupPage.RadioOptionValue.OptionD,
                RadioButtonGroupPage.RadioOptionValue.OptionA);

            TestRadioButtonGroup(
                page.RadioOptionsByCssAndValue,
                RadioButtonGroupPage.RadioOptionLabel.OptionB,
                RadioButtonGroupPage.RadioOptionLabel.OptionC);

            Assert.Throws<NoSuchElementException>(() =>
                page.RadioOptionsByClassAndValue.Set(RadioButtonGroupPage.RadioOptionValue.MissingValue));

            Assert.Throws<ArgumentNullException>(() =>
                page.RadioOptionsByNameAndLabel.Set(null));
        }

        [Test]
        public void RadioButtonGroup_String()
        {
            page.VerticalItems.VerifyEquals("Item 1");

            TestRadioButtonGroup(
                page.VerticalItems,
                "Item 2",
                "Item 5");

            Assert.Throws<NoSuchElementException>(() =>
                page.VerticalItems.Set("Item 999"));

            Assert.Throws<ArgumentNullException>(() =>
                page.VerticalItems.Set(null));
        }

        private void TestRadioButtonGroup<T>(RadioButtonGroup<T, RadioButtonGroupPage> group, T value1, T value2)
        {
            T actualValue;

            group.VerifyExists();
            group.Set(value1);
            group.VerifyEquals(value1);
            group.Get(out actualValue);
            Assert.That(actualValue, Is.EqualTo(value1));

            group.Set(value2);
            group.VerifyNotEqual(value1);
            group.VerifyEquals(value2);
            group.Get(out actualValue);
            Assert.That(actualValue, Is.EqualTo(value2));
        }
    }
}
