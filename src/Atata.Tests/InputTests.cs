using System;
using System.IO;
using NUnit.Framework;

namespace Atata.Tests
{
    public class InputTests : UITestFixture
    {
        private InputPage page;

        protected override void OnSetUp()
        {
            page = Go.To<InputPage>();
        }

        [Test]
        public void TextInput()
        {
            var control = page.TextInput;

            VerifyEquals(control, null);

            SetAndVerifyValues(control, "Text1", null, "Text2");

            VerifyDoesNotEqual(control, "Text3");

            control.Append("0");
            control.Should.Equal("Text20");
            control.Clear();
            control.Should.BeNull();
        }

        [Test]
        public void Input_Enum()
        {
            var control = page.EnumTextInput;

            SetAndVerifyValues(control, InputPage.Option.OptionA, InputPage.Option.OptionC);

            VerifyDoesNotEqual(control, InputPage.Option.OptionD);
        }

        [Test]
        public void Input_NullableEnum()
        {
            var control = page.NullableEnumTextInput;

            VerifyEquals(control, null);

            SetAndVerifyValues(control, InputPage.Option.OptionD, InputPage.Option.OptionA);

            VerifyDoesNotEqual(control, InputPage.Option.OptionB);
        }

        [Test]
        public void Input_Int()
        {
            var control = page.IntTextInput;

            VerifyEquals(control, null);

            SetAndVerifyValues(control, 45, null, 57);

            VerifyDoesNotEqual(control, 59);

            control.Should.BeGreater(55);
            control.Should.BeLess(60);
            control.Should.BeInRange(50, 60);
        }

        [Test]
        public void NumberInput()
        {
            var control = page.NumberInput;

            VerifyEquals(control, null);

            SetAndVerifyValues(control, 45, null, 57);

            VerifyDoesNotEqual(control, 59);

            control.Should.BeGreater(55);
            control.Should.BeLess(60);
            control.Should.BeInRange(50, 60);
            control.Get(out int? intNumber);

            Assert.That(intNumber, Is.EqualTo(57));

            control.SetRandom(out intNumber);
            control.Should.Equal(intNumber);
        }

        [Test]
        public void Input_File()
        {
            var control = page.FileInput;

            VerifyEquals(control, null);

            string assemblyName = $"{GetType().Assembly.GetName().Name}.dll";

            control.Set(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName));

            control.Should.EndWith(assemblyName);

            control.Clear();
            control.Should.BeNull();
        }
    }
}
