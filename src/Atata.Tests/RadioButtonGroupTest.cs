using NUnit.Framework;
using OpenQA.Selenium;

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
                RadioButtonGroupPage.RadioOptionValue.OptionB,
                RadioButtonGroupPage.RadioOptionValue.OptionC);

            Assert.Throws<NoSuchElementException>(() =>
                page.RadioOptionsByCssAndValue.Set(RadioButtonGroupPage.RadioOptionValue.MissingValue));
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
        }

        private void TestRadioButtonGroup<T>(RadioButtonGroup<T, RadioButtonGroupPage> group, T value1, T value2)
        {
            group.VerifyExists();
            group.Set(value1);
            group.VerifyEquals(value1);
            group.Set(value2);
            group.VerifyNotEqual(value1);
            group.VerifyEquals(value2);
        }
    }
}
