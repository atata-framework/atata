using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class InputTest : TestBase
    {
        private InputPage page;

        protected override void OnSetUp()
        {
            page = Go.To<InputPage>();
        }

        [Test]
        public void Input_Text()
        {
            VerifyEquals(page.TextInput, null);

            SetAndVerifyValues(page.TextInput, "Text1", null, "Text2");

            VerifyDoesNotEqual(page.TextInput, "Text3");
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
        }
    }
}
