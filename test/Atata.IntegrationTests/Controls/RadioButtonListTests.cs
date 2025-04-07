namespace Atata.IntegrationTests.Controls;

public class RadioButtonListTests : WebDriverSessionTestSuite
{
    private RadioButtonListPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<RadioButtonListPage>();

    [Test]
    public void OfEnumType()
    {
        _page.ByNameAndLabel.Should.Be(null)
            .ByNameAndLabel.Should.BeNull();

        _page.ByClassAndValue.Should.Be(RadioButtonListPage.ByValue.None);

        SetAndVerifyValues(
            _page.ByNameAndLabel,
            RadioButtonListPage.ByLabel.OptionC,
            RadioButtonListPage.ByLabel.OptionB);

        SetAndVerifyValues(
            _page.ByClassAndValue,
            RadioButtonListPage.ByValue.OptionD,
            RadioButtonListPage.ByValue.OptionA);

        SetAndVerifyValues(
            _page.ByCssAndValue,
            RadioButtonListPage.ByLabel.OptionB,
            RadioButtonListPage.ByLabel.OptionC);

        Assert.Throws<ElementNotFoundException>(() =>
            _page.ByClassAndValue.Set(RadioButtonListPage.ByValue.MissingValue));

        Assert.Throws<ArgumentNullException>(() =>
            _page.ByNameAndLabel.Set(null));
    }

    [Test]
    public void OfStringType()
    {
        _page.VerticalItems.Should.Be("Item 1");
        _page.VerticalItems.Should.Not.BeNull();

        SetAndVerifyValues(_page.VerticalItems, "Item 2", "Item 5");
        SetAndVerifyValues(_page.VerticalItemsByFieldSet, "Item 3", "Item 1");

        Assert.Throws<ElementNotFoundException>(() =>
            _page.VerticalItems.Set("Item 999"));

        Assert.Throws<ArgumentNullException>(() =>
            _page.VerticalItems.Set(null!));
    }

    [Test]
    public void OfIntType()
    {
        _page.IntegerItems.Should.BeNull();

        SetAndVerifyValues(_page.IntegerItems, 2, 3);

        Assert.Throws<ElementNotFoundException>(() =>
            _page.IntegerItems.Set(9));

        Assert.Throws<ArgumentNullException>(() =>
            _page.VerticalItems.Set(null!));
    }

    [Test]
    public void OfDecimalType()
    {
        _page.DecimalItems.Should.BeNull();

        SetAndVerifyValues(_page.DecimalItems, 1000, 2500);
        SetAndVerifyValues(_page.DecimalItems, 3210.50m, 4310.10m);

        Assert.Throws<ElementNotFoundException>(() =>
            _page.DecimalItems.Set(918.76m));

        Assert.Throws<ArgumentNullException>(() =>
            _page.VerticalItems.Set(null!));
    }

    [Test]
    public void OfBoolType()
    {
        var control = _page.BoolItems;

        control.Should.BeNull();

        SetAndVerifyValues(control, false, true);
    }

    [Test]
    public void OfStringType_WithFindItemByParentContentAttribute() =>
        VerifyRegularStringBasedRadioButtonList(_page.TextInParentItems);

    [Test]
    public void RadioButtonList_String_FindItemByFollowingSiblingContentAttribute() =>
        VerifyRegularStringBasedRadioButtonList(_page.TextInFollowingSiblingItems);

    [Test]
    public void RadioButtonList_String_FindItemByPrecedingSiblingContentAttribute() =>
        VerifyRegularStringBasedRadioButtonList(_page.TextInPrecedingSiblingItems);

    [Test]
    public void ToggleRandom() =>
        _page
            .VerticalItems.Get(out var selectedValue)
            .VerticalItems.ToggleRandom()
            .VerticalItems.Should.Not.Be(selectedValue);

    private static void VerifyRegularStringBasedRadioButtonList(RadioButtonList<string, RadioButtonListPage> control)
    {
        control.Should.BeNull();

        SetAndVerifyValues(control, "Option B", "Option C");

        Assert.Throws<ElementNotFoundException>(() =>
            control.Set("Option Z"));

        Assert.Throws<ArgumentNullException>(() =>
            control.Set(null!));
    }
}
