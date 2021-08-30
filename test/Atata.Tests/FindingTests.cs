using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class FindingTests : UITestFixture
    {
        private FindingPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<FindingPage>();
        }

        [Test]
        public void Find_ByIndex()
        {
            VerifyRadioButton(_page.OptionCByIndex);
        }

        [Test]
        public void Find_ByNameAndIndex()
        {
            VerifyRadioButton(_page.OptionCByName);
        }

        [Test]
        public void Find_ByCssAndIndex()
        {
            VerifyRadioButton(_page.OptionCByCss);
        }

        [Test]
        public void Find_ByXPathAndIndex()
        {
            VerifyRadioButton(_page.OptionCByXPath);
        }

        [Test]
        public void Find_ByXPathAndIndex_Condition()
        {
            VerifyRadioButton(_page.OptionCByXPathCondition);
        }

        [Test]
        public void Find_ByXPathAndIndex_Attribute()
        {
            VerifyRadioButton(_page.OptionCByXPathAttribute);
        }

        [Test]
        public void Find_ByAttributeAndIndex()
        {
            VerifyRadioButton(_page.OptionCByName);
        }

        [Test]
        public void Find_ByClassAndIndex()
        {
            VerifyRadioButton(_page.OptionCByClass);
        }

        [Test]
        public void Find_Last()
        {
            VerifyRadioButton(_page.OptionDAsLast, "OptionD");
        }

        [Test]
        public void Find_Visible()
        {
            _page.VisibleInput.Should.Exist().
                VisibleInput.Should.BeVisible().
                FailDisplayNoneInput.Should.Not.Exist().
                FailOpacity0Input.Should.Not.Exist();

            AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
                _page.FailDisplayNoneInput.Should.AtOnce.Exist());

            AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
                _page.FailOpacity0Input.Should.AtOnce.Exist());
        }

        [Test]
        public void Find_Hidden()
        {
            _page.DisplayNoneInput.Should.Exist().
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

            Assert.That(_page.FailDisplayNoneInput.Exists(SearchOptions.Hidden()), Is.True);
        }

        [Test]
        public void Find_ByCss_OuterXPath()
        {
            VerifyRadioButton(_page.OptionCByCssWithOuterXPath);
        }

        [Test]
        public void Find_ByCss_OuterXPath_Missing()
        {
            VerifyNotExist(_page.MissingOptionByCssWithOuterXPath);
        }

        [Test]
        public void Find_ByCss_Missing()
        {
            VerifyNotExist(_page.MissingOptionByCss);
        }

        [Test]
        public void Find_ByLabel_Missing()
        {
            VerifyNotExist(_page.MissingOptionByLabel);
        }

        [Test]
        public void Find_ByXPath_Missing()
        {
            VerifyNotExist(_page.MissingOptionByXPath);
        }

        [Test]
        public void Find_ById_Missing()
        {
            VerifyNotExist(_page.MissingOptionById);
        }

        [Test]
        public void Find_ByColumnHeader_Missing()
        {
            VerifyNotExist(_page.MissingOptionByColumnHeader);
        }

        [Test]
        public void Find_ByScript()
        {
            VerifyRadioButton(_page.OptionByScript, "OptionB");
        }

        [Test]
        public void Find_ByScript_WithIndex()
        {
            VerifyRadioButton(_page.OptionByScriptWithIndex, "OptionC");
        }

        [Test]
        public void Find_ByScript_WithIncorrectIndex()
        {
            VerifyNotExist(_page.OptionByScriptWithIncorrectIndex);
        }

        [Test]
        public void Find_ByScript_Missing()
        {
            VerifyNotExist(_page.OptionByScriptMissing);
        }

        [Test]
        public void Find_ByScript_WithInvalidScript()
        {
            IWebElement element = null;

            WebDriverException exception = Assert.Throws<WebDriverException>(() =>
                element = _page.OptionByScriptWithInvalidScript.Scope);

            Assert.That(exception.Message, Does.StartWith("javascript error:"));

            AssertThrowsWithInnerException<AssertionException, WebDriverException>(() =>
                _page.OptionByScriptWithInvalidScript.Should.AtOnce.Exist());
        }

        [Test]
        public void Find_ByScript_WithIncorrectScriptResult()
        {
            IWebElement element = null;

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                element = _page.OptionByScriptWithIncorrectScriptResult.Scope);

            Assert.That(exception.Message, Does.Contain("I am not OK."));

            AssertThrowsWithInnerException<AssertionException, InvalidOperationException>(() =>
                _page.OptionByScriptWithIncorrectScriptResult.Should.AtOnce.Exist());
        }

        [Test]
        public void Find_ByDescendantId()
        {
            _page.ControlByDescendantId.Should.HaveClass("custom-control");
        }

        [Test]
        public void Find_ByDescendantId_Missing()
        {
            VerifyNotExist(_page.ControlByDescendantIdMissing);
        }

        [Test]
        public void Find_ControlDefinition_MultipleClasses()
        {
            _page.SpanWithMultipleClasses.Should.Equal("Span with multiple classes");
        }

        [Test]
        public void Find_ControlDefinition_MultipleClasses_Missing()
        {
            VerifyNotExist(_page.MissingSpanWithMultipleClasses);
        }

        [Test]
        public void Find_FindAttributeAtParentLevel()
        {
            var control = _page.OptionCAsCustom;
            VerifyValue(control, "OptionC");
        }

        [Test]
        public void Find_AfterPushToMetadata()
        {
            var control = _page.OptionCByIndex;
            VerifyValue(control, "OptionC");

            control.Metadata.Push(new FindByValueAttribute("OptionB"));
            VerifyValue(control, "OptionB");

            control.Metadata.Push(new FindByValueAttribute("OptionC"));
            VerifyValue(control, "OptionC");
        }

        [Test]
        public void Find_AfterPushToDifferentLevelsOfMetadata()
        {
            var control = _page.OptionDAsCustom;
            VerifyValue(control, "OptionD");

            _page.Metadata.Push(new FindByValueAttribute("OptionC"));
            VerifyValue(control, "OptionD");

            _page.Metadata.Push(new FindByValueAttribute("OptionC") { TargetName = nameof(FindingPage.OptionDAsCustom) });
            VerifyValue(control, "OptionC");

            control.Metadata.Push(new FindByValueAttribute("OptionB"));
            VerifyValue(control, "OptionB");
        }

        [TestCase(0)]
        [TestCase(2)]
        [TestCase(7)]
        public void Find_Timeout(double timeout)
        {
            var control = _page.MissingOptionById;
            control.Metadata.Get<FindAttribute>().Timeout = timeout;

            using (StopwatchAsserter.WithinSeconds(timeout))
                Assert.Throws<NoSuchElementException>(() =>
                    control.Click());
        }

        private static void VerifyRadioButton(RadioButton<FindingPage> radioButton, string expectedValue = "OptionC")
        {
            VerifyValue(radioButton, expectedValue);
            radioButton.Should.BeUnchecked();
            radioButton.Check();
            radioButton.Should.BeChecked();
        }

        private static void VerifyValue<TOwner>(UIComponent<TOwner> component, string expectedValue)
            where TOwner : PageObject<TOwner>
        {
            Assert.That(component.Attributes.GetValue("value"), Is.EqualTo(expectedValue));
        }

        private static void VerifyNotExist<TOwner>(UIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            component.Should.Not.Exist();

            AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
                component.Should.AtOnce.Exist());
        }
    }
}
