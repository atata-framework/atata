namespace Atata.IntegrationTests.Controls;

public class CheckBoxListTests : WebDriverSessionTestSuite
{
    private CheckBoxListPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<CheckBoxListPage>();

    [Test]
    public void OfEnumType()
    {
        _page.ByIdAndLabel.Should.Be(CheckBoxListPage.Options.None);

        SetAndVerifyValues(
            _page.ByIdAndLabel,
            CheckBoxListPage.Options.OptionC | CheckBoxListPage.Options.OptionD,
            CheckBoxListPage.Options.OptionB);

        SetAndVerifyValues(
            _page.ByXPathAndValue,
            CheckBoxListPage.Options.OptionA,
            CheckBoxListPage.Options.OptionsDF);

        _page.ByIdAndLabel.Should.Not.HaveChecked(CheckBoxListPage.Options.OptionA)
            .ByIdAndLabel.Should.HaveChecked(CheckBoxListPage.Options.OptionD | CheckBoxListPage.Options.OptionF);

        SetAndVerifyValues(
            _page.ByXPathAndValue,
            CheckBoxListPage.Options.None,
            CheckBoxListPage.Options.OptionA);

        _page.ByIdAndLabel.Check(CheckBoxListPage.Options.OptionD);
        _page.ByXPathAndValue.Should.Be(CheckBoxListPage.Options.OptionA | CheckBoxListPage.Options.OptionD);

        _page.ByXPathAndValue.Uncheck(CheckBoxListPage.Options.OptionA);
        _page.ByIdAndLabel.Should.HaveChecked(CheckBoxListPage.Options.OptionD);

        Assert.Throws<AssertionException>(() =>
            _page.ByIdAndLabel.Should.AtOnce.Not.HaveChecked(CheckBoxListPage.Options.OptionD));

        Assert.Throws<AssertionException>(() =>
            _page.ByIdAndLabel.Should.AtOnce.HaveChecked(CheckBoxListPage.Options.OptionA));

        Assert.Throws<ElementNotFoundException>(() =>
            _page.ByIdAndLabel.Set(CheckBoxListPage.Options.MissingValue));
    }

    [Test]
    public void OfEnumType_WithFindItemByLabelAttribute()
    {
        var sut = _page.ByFieldsetAndLabelUsingId;

        sut.Should.Be(CheckBoxListPage.Options.None);

        SetAndVerifyValues(
            sut,
            CheckBoxListPage.Options.OptionC | CheckBoxListPage.Options.OptionD,
            CheckBoxListPage.Options.OptionB);

        sut.Should.Not.HaveChecked(CheckBoxListPage.Options.OptionA);
        sut.Should.HaveChecked(CheckBoxListPage.Options.OptionB);

        SetAndVerifyValues(
            sut,
            CheckBoxListPage.Options.None,
            CheckBoxListPage.Options.OptionA);

        sut.Check(CheckBoxListPage.Options.OptionD);
        sut.Should.Be(CheckBoxListPage.Options.OptionA | CheckBoxListPage.Options.OptionD);

        sut.Uncheck(CheckBoxListPage.Options.OptionA);
        sut.Should.HaveChecked(CheckBoxListPage.Options.OptionD);

        Assert.Throws<AssertionException>(() =>
            sut.Should.AtOnce.Not.HaveChecked(CheckBoxListPage.Options.OptionD));

        Assert.Throws<AssertionException>(() =>
            sut.Should.AtOnce.HaveChecked(CheckBoxListPage.Options.OptionA));

        Assert.Throws<ElementNotFoundException>(() =>
            sut.Set(CheckBoxListPage.Options.MissingValue));
    }
}
