namespace Atata.IntegrationTests.Finding;

public class FindingTests : WebDriverSessionTestSuite
{
    private FindingPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<FindingPage>();

    [Test]
    public void ByIndex() =>
        VerifyRadioButton(_page.OptionCByIndex);

    [Test]
    public void ByNameAndIndex() =>
        VerifyRadioButton(_page.OptionCByName);

    [Test]
    public void ByCssAndIndex() =>
        VerifyRadioButton(_page.OptionCByCss);

    [Test]
    public void ByXPathAndIndex() =>
        VerifyRadioButton(_page.OptionCByXPath);

    [Test]
    public void ByXPathAndIndex_Condition() =>
        VerifyRadioButton(_page.OptionCByXPathCondition);

    [Test]
    public void ByXPathAndIndex_Attribute() =>
        VerifyRadioButton(_page.OptionCByXPathAttribute);

    [Test]
    public void ByAttributeAndIndex() =>
        VerifyRadioButton(_page.OptionCByName);

    [Test]
    public void ByClassAndIndex() =>
        VerifyRadioButton(_page.OptionCByClass);

    [Test]
    public void ByLabel_WithId() =>
        _page.UserNameByLabel.DomProperties.Id.Should.Be("user-name");

    [Test]
    public void ByLabel_AndIdXPathForLabel_WithId() =>
        _page.ControlWithIdXPathForLabel.DomClasses.Should.Contain("custom-control");

    [Test]
    public void ByLabelAndIndex_WithId() =>
        _page.UserNameByLabelAndIndex.DomProperties.Id.Should.Be("user-name");

    [Test]
    public void ByLabel_WithoutId() =>
        VerifyRadioButton(_page.OptionCByLabel, "OptionC");

    [Test]
    public void ByLabelAndIndex_WithoutId() =>
        VerifyRadioButton(_page.OptionCByLabelWithIndex, "OptionC");

    [Test]
    public void Last() =>
        VerifyRadioButton(_page.OptionDAsLast, "OptionD");

    [Test]
    public void Visible()
    {
        _page.VisibleInput.Should.BePresent()
            .VisibleInput.Should.BeVisible()
            .FailDisplayNoneInput.Should.Not.BePresent()
            .FailOpacity0Input.Should.Not.BePresent();

        AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
            _page.FailDisplayNoneInput.Should.AtOnce.BePresent());

        AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
            _page.FailOpacity0Input.Should.AtOnce.BePresent());
    }

    [Test]
    public void Hidden()
    {
        _page.DisplayNoneInput.Should.BePresent()
            .DisplayNoneInput.Should.BeHidden()
            .HiddenInput.Should.BePresent()
            .HiddenInput.Should.BeHidden()
            .CollapseInput.Should.BePresent()
            .CollapseInput.Should.BeHidden()
            .Opacity0Input.Should.BePresent()
            .Opacity0Input.Should.BeHidden()
            .TypeHiddenInput.Should.BePresent()
            .TypeHiddenInput.Should.BeHidden()
            .TypeHiddenInputWithDeclaredDefinition.Should.BePresent()
            .TypeHiddenInputWithDeclaredDefinition.Should.BeHidden();

        Assert.That(_page.FailDisplayNoneInput.Exists(SearchOptions.Hidden()), Is.True);
    }

    [Test]
    public void ByCss_OuterXPath() =>
        VerifyRadioButton(_page.OptionCByCssWithOuterXPath);

    [Test]
    public void ByCss_OuterXPath_Missing() =>
        VerifyNotPresent(_page.MissingOptionByCssWithOuterXPath);

    [Test]
    public void ByCss_Missing() =>
        VerifyNotPresent(_page.MissingOptionByCss);

    [Test]
    public void ByLabel_Missing() =>
        VerifyNotPresent(_page.MissingOptionByLabel);

    [Test]
    public void ByXPath_Missing() =>
        VerifyNotPresent(_page.MissingOptionByXPath);

    [Test]
    public void ById_Missing() =>
        VerifyNotPresent(_page.MissingOptionById);

    [Test]
    public void ByColumnHeader_Missing() =>
        VerifyNotPresent(_page.MissingOptionByColumnHeader);

    [Test]
    public void ByScript() =>
        VerifyRadioButton(_page.OptionByScript, "OptionB");

    [Test]
    public void ByScript_WithIndex() =>
        VerifyRadioButton(_page.OptionByScriptWithIndex, "OptionC");

    [Test]
    public void ByScript_WithIncorrectIndex() =>
        VerifyNotPresent(_page.OptionByScriptWithIncorrectIndex);

    [Test]
    public void ByScript_Missing() =>
        VerifyNotPresent(_page.OptionByScriptMissing);

    [Test]
    public void ByScript_WithInvalidScript()
    {
        var exception = Assert.Throws<JavaScriptException>(() =>
            _ = _page.OptionByScriptWithInvalidScript.Scope)!;

        Assert.That(exception.Message, Does.StartWith("javascript error:"));

        AssertThrowsWithInnerException<AssertionException, JavaScriptException>(() =>
            _page.OptionByScriptWithInvalidScript.Should.AtOnce.BePresent());
    }

    [Test]
    public void ByScript_WithIncorrectScriptResult()
    {
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
            _ = _page.OptionByScriptWithIncorrectScriptResult.Scope)!;

        Assert.That(exception.Message, Does.Contain("I am not OK."));

        AssertThrowsWithInnerException<AssertionException, InvalidOperationException>(() =>
            _page.OptionByScriptWithIncorrectScriptResult.Should.AtOnce.BePresent());
    }

    [Test]
    public void ByDescendantId() =>
        _page.ControlByDescendantId.Should.HaveClass("custom-control");

    [Test]
    public void ByDescendantId_Missing() =>
        VerifyNotPresent(_page.ControlByDescendantIdMissing);

    [Test]
    public void ByControlDefinition_MultipleClasses() =>
        _page.SpanWithMultipleClasses.Should.Be("Span with multiple classes");

    [Test]
    public void ByControlDefinition_MultipleClasses_Missing() =>
        VerifyNotPresent(_page.MissingSpanWithMultipleClasses);

    [Test]
    public void WhenFindAttributeAtParentLevel() =>
        VerifyValue(_page.OptionCAsCustom, "OptionC");

    [Test]
    public void AfterPushToMetadata()
    {
        var sut = _page.OptionCByIndex;
        VerifyValue(sut, "OptionC");

        sut.Metadata.Push(new FindByValueAttribute("OptionB"));
        VerifyValue(sut, "OptionB");

        sut.Metadata.Push(new FindByValueAttribute("OptionC"));
        VerifyValue(sut, "OptionC");
    }

    [Test]
    public void AfterPushToDifferentLevelsOfMetadata()
    {
        var sut = _page.OptionDAsCustom;
        VerifyValue(sut, "OptionD");

        _page.Metadata.Push(new FindByValueAttribute("OptionC"));
        VerifyValue(sut, "OptionD");

        _page.Metadata.Push(new FindByValueAttribute("OptionC") { TargetName = nameof(FindingPage.OptionDAsCustom) });
        VerifyValue(sut, "OptionC");

        sut.Metadata.Push(new FindByValueAttribute("OptionB"));
        VerifyValue(sut, "OptionB");
    }

    [TestCase(0)]
    [TestCase(2)]
    [TestCase(7)]
    public void WithTimeout(double timeout)
    {
        var sut = _page.MissingOptionById;
        sut.Metadata.Get<FindAttribute>()!.Timeout = timeout;

        using (StopwatchAsserter.WithinSeconds(timeout))
            Assert.Throws<ElementNotFoundException>(() =>
                sut.Click());
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
        =>
        Assert.That(component.DomProperties.GetValue("value"), Is.EqualTo(expectedValue));

    private static void VerifyNotPresent<TOwner>(UIComponent<TOwner> component)
        where TOwner : PageObject<TOwner>
    {
        component.Should.Not.BePresent();

        AssertThrowsAssertionExceptionWithUnableToLocateMessage(() =>
            component.Should.AtOnce.BePresent());
    }
}
