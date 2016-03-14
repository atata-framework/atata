using NUnit.Framework;
using System;

namespace Atata.Tests
{
    [TestFixture]
    public class RadioButtonGroupTest : TestBase
    {
        [Test]
        public void RadioButtonGroup_Enum()
        {
            var page = GoTo<RadioButtonGroupPage>();

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
        }

        private void TestRadioButtonGroup<T>(RadioButtonGroup<T, RadioButtonGroupPage> group, T value1, T value2)
            where T : struct, IComparable, IFormattable
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
