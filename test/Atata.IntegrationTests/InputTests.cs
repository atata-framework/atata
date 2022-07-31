using System;
using System.IO;
using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class InputTests : UITestFixture
    {
        private InputPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<InputPage>();
        }

        [Test]
        public void TextInput()
        {
            VerifyStringInput(_page.TextInput);
        }

        [Test]
        public void Input_Enum()
        {
            var control = _page.EnumTextInput;

            SetAndVerifyValues(control, InputPage.Option.OptionA, InputPage.Option.OptionC);

            VerifyDoesNotEqual(control, InputPage.Option.OptionD);
        }

        [Test]
        public void Input_NullableEnum()
        {
            var control = _page.NullableEnumTextInput;

            VerifyEquals(control, null);

            SetAndVerifyValues(control, InputPage.Option.OptionD, InputPage.Option.OptionA);

            VerifyDoesNotEqual(control, InputPage.Option.OptionB);
        }

        [Test]
        public void Input_Int()
        {
            var control = _page.IntTextInput;

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
            var control = _page.NumberInput;

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
        public void FileInput()
        {
            var control = _page.FileInput;

            control.Should.Exist();
            control.Should.BeVisible();

            TestFileInput(control);
        }

        [Test]
        public void FileInput_Hidden()
        {
            var control = _page.HiddenFileInput;

            control.Should.Exist();
            control.Should.BeHidden();

            TestFileInput(control);
        }

        [Test]
        public void FileInput_Transparent()
        {
            var control = _page.TransparentFileInput;

            control.Should.Exist();
            control.Should.BeHidden();

            TestFileInput(control);
        }

        private void TestFileInput(FileInput<InputPage> control)
        {
            VerifyEquals(control, string.Empty);

            string file1Name = $"{GetType().Assembly.GetName().Name}.dll";
            control.Set(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file1Name));
            control.Should.EndWith(file1Name);

            string file2Name = $"{typeof(OrdinaryPage).Assembly.GetName().Name}.dll";
            control.Set(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file2Name));
            control.Should.EndWith(file2Name);

            control.Clear();
            control.Should.BeEmpty();
        }

        [Test]
        public void TelInput()
        {
            var control = _page.TelInput;

            VerifyEquals(control, string.Empty);

            SetAndVerifyValues(control, "152-154-1456", string.Empty, "+11521541456");

            VerifyDoesNotEqual(control, "2345325523");

            control.Clear();
            control.Should.BeEmpty();
        }

        [Test]
        public void SearchInput()
        {
            VerifyStringInput(_page.SearchInput);
        }

        [Test]
        public void EmailInput()
        {
            VerifyStringInput(_page.EmailInput);
        }

        [Test]
        public void UrlInput()
        {
            VerifyStringInput(_page.UrlInput);
        }

        private static void VerifyStringInput(Input<string, InputPage> control)
        {
            VerifyEquals(control, string.Empty);

            SetAndVerifyValues(control, "Text1", string.Empty, "Text2");

            VerifyDoesNotEqual(control, "Text3");

            control.Type("0");
            control.Should.Equal("Text20");
            control.Clear();
            control.Should.BeEmpty();

            control.Type("1");
            control.Set(null);
            control.Should.BeEmpty();
        }
    }
}
