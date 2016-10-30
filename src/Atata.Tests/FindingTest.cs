using NUnit.Framework;

namespace Atata.Tests
{
    public class FindingTest : AutoTest
    {
        private FindingPage page;

        protected override void OnSetUp()
        {
            page = Go.To<FindingPage>();
        }

        [Test]
        public void Find_ByIndex()
        {
            VerifyRadioButton(page.OptionCByIndex);
        }

        [Test]
        public void Find_ByNameAndIndex()
        {
            VerifyRadioButton(page.OptionCByName);
        }

        [Test]
        public void Find_ByCssAndIndex()
        {
            VerifyRadioButton(page.OptionCByCss);
        }

        [Test]
        public void Find_ByXPathAndIndex()
        {
            VerifyRadioButton(page.OptionCByXPath);
        }

        [Test]
        public void Find_ByXPathAndIndex_Conditional()
        {
            VerifyRadioButton(page.OptionCByXPathInner);
        }

        [Test]
        public void Find_ByAttributeAndIndex()
        {
            VerifyRadioButton(page.OptionCByName);
        }

        [Test]
        public void Find_ByClassAndIndex()
        {
            VerifyRadioButton(page.OptionCByClass);
        }

        private void VerifyRadioButton(RadioButton<FindingPage> radioButton)
        {
            VerifyValue(radioButton, "OptionC");
            radioButton.Should.BeUnchecked();
            radioButton.Check();
            radioButton.Should.BeChecked();
        }

        private void VerifyValue(UIComponent component, string expectedValue)
        {
            Assert.That(component.Attributes["value"], Is.EqualTo(expectedValue));
        }
    }
}
