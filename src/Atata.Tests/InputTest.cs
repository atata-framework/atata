using NUnit.Framework;

namespace Atata.Tests
{
    public class InputTest : AutoTest
    {
        private InputPage page;

        protected override void OnSetUp()
        {
            page = Go.To<InputPage>();
        }

        [Test]
        public void TextInput()
        {
            VerifyEquals(page.TextInput, null);

            SetAndVerifyValues(page.TextInput, "Text1", null, "Text2");

            VerifyDoesNotEqual(page.TextInput, "Text3");

            page.TextInput.Append("0").
                TextInput.Should.Equal("Text20").
                TextInput.Clear().
                TextInput.Should.BeNull();
        }

        [Test]
        public void Input_Enum()
        {
            SetAndVerifyValues(page.EnumTextInput, InputPage.Option.OptionA, InputPage.Option.OptionC);

            VerifyDoesNotEqual(page.EnumTextInput, InputPage.Option.OptionD);
        }

        [Test]
        public void Input_NullableEnum()
        {
            VerifyEquals(page.NullableEnumTextInput, null);

            SetAndVerifyValues(page.NullableEnumTextInput, InputPage.Option.OptionD, InputPage.Option.OptionA);

            VerifyDoesNotEqual(page.NullableEnumTextInput, InputPage.Option.OptionB);
        }

        [Test]
        public void Input_Int()
        {
            VerifyEquals(page.IntTextInput, null);

            SetAndVerifyValues(page.IntTextInput, 45, null, 57);

            VerifyDoesNotEqual(page.IntTextInput, 59);

            page.IntTextInput.Should.BeGreater(55).
                IntTextInput.Should.BeLess(60).
                IntTextInput.Should.BeInRange(50, 60);
        }

        [Test]
        public void NumberInput()
        {
            VerifyEquals(page.NumberInput, null);

            SetAndVerifyValues(page.NumberInput, 45, null, 57);

            VerifyDoesNotEqual(page.NumberInput, 59);

            int? intNumber;

            page.NumberInput.Should.BeGreater(55).
                NumberInput.Should.BeLess(60).
                NumberInput.Should.BeInRange(50, 60).
                NumberInput.Get(out intNumber);

            Assert.That(intNumber, Is.EqualTo(57));

            page.NumberInput.SetRandom(out intNumber).
                NumberInput.Should.Equal(intNumber);
        }
    }
}
