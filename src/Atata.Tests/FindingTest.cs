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
        public void Find_ByXPathAndIndex_Condition()
        {
            VerifyRadioButton(page.OptionCByXPathCondition);
        }

        [Test]
        public void Find_ByXPathAndIndex_Attribute()
        {
            VerifyRadioButton(page.OptionCByXPathAttribute);
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

        [Test]
        public void Find_Last()
        {
            VerifyRadioButton(page.OptionDAsLast, "OptionD");
        }

        private void VerifyRadioButton(RadioButton<FindingPage> radioButton, string expectedValue = "OptionC")
        {
            VerifyValue(radioButton, expectedValue);
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
