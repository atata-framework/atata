using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class FindingTests : UITestFixture
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

        [Test]
        public void Find_Visible()
        {
            page.VisibleInput.Should.Exist().
                VisibleInput.Should.BeVisible().
                FailDisplayNoneInput.Should.Not.Exist().
                FailOpacity0Input.Should.Not.Exist();

            Assert.Throws<NoSuchElementException>(() =>
                page.FailDisplayNoneInput.Should.AtOnce.Exist());

            Assert.Throws<NoSuchElementException>(() =>
                page.FailOpacity0Input.Should.AtOnce.Exist());
        }

        [Test]
        public void Find_Hidden()
        {
            page.DisplayNoneInput.Should.Exist().
                DisplayNoneInput.Should.BeHidden().
                HiddenInput.Should.Exist().
                HiddenInput.Should.BeHidden().
                CollapseInput.Should.Exist().
                CollapseInput.Should.BeHidden().
                Opacity0Input.Should.Exist().
                Opacity0Input.Should.BeHidden().
                TypeHiddenInput.Should.Exist().
                TypeHiddenInput.Should.BeHidden().
                TypeHiddenInputWithDeclaredDefinition.Should.Exist().
                TypeHiddenInputWithDeclaredDefinition.Should.BeHidden();

            Assert.That(page.FailDisplayNoneInput.Exists(SearchOptions.Hidden()), Is.True);
        }

        [Test]
        public void Find_ByCss_OuterXPath()
        {
            VerifyRadioButton(page.OptionCByCssWithOuterXPath);
        }

        [Test]
        public void Find_ByCss_OuterXPath_Missing()
        {
            VerifyNotExist(page.MissingOptionByCssWithOuterXPath);
        }

        [Test]
        public void Find_ByCss_Missing()
        {
            VerifyNotExist(page.MissingOptionByCss);
        }

        [Test]
        public void Find_ByLabel_Missing()
        {
            VerifyNotExist(page.MissingOptionByLabel);
        }

        [Test]
        public void Find_ByXPath_Missing()
        {
            VerifyNotExist(page.MissingOptionByXPath);
        }

        [Test]
        public void Find_ById_Missing()
        {
            VerifyNotExist(page.MissingOptionById);
        }

        [Test]
        public void Find_ByColumnHeader_Missing()
        {
            VerifyNotExist(page.MissingOptionByColumnHeader);
        }

        private void VerifyRadioButton(RadioButton<FindingPage> radioButton, string expectedValue = "OptionC")
        {
            VerifyValue(radioButton, expectedValue);
            radioButton.Should.BeUnchecked();
            radioButton.Check();
            radioButton.Should.BeChecked();
        }

        private void VerifyValue<TOwner>(UIComponent<TOwner> component, string expectedValue)
            where TOwner : PageObject<TOwner>
        {
            Assert.That(component.Attributes.GetValue("value"), Is.EqualTo(expectedValue));
        }

        private void VerifyNotExist<TOwner>(UIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            component.Should.Not.Exist();

            Assert.Throws<NoSuchElementException>(() =>
                component.Should.AtOnce.Exist());
        }
    }
}
