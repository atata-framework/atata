using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class InputTest : TestBase
    {
        private InputPage page;

        protected override void OnSetUp()
        {
            page = GoTo<InputPage>();
        }

        [Test]
        public void Input_Text()
        {
            SetAndVerifyValues(page.TextInput, "Text1", "Text2");

            VerifyNotEqual(page.TextInput, "Text3");
        }

        [Test]
        public void Input_Enum()
        {
            SetAndVerifyValues(page.EnumTextInput, InputPage.Option.OptionA, InputPage.Option.OptionC);

            VerifyNotEqual(page.EnumTextInput, InputPage.Option.OptionD);
        }

        [Test]
        public void Input_Int()
        {
            SetAndVerifyValues(page.IntTextInput, 45, 57);

            VerifyNotEqual(page.IntTextInput, 59);
        }
    }
}
