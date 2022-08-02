namespace Atata.IntegrationTests.Controls;

public class SelectTests : UITestFixture
{
    private SelectPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<SelectPage>();

    [Test]
    public void OfStringType()
    {
        var sut = _page.TextSelect;

        VerifyEquals(sut, "--select--");
        sut.SelectedIndex.Should.Equal(0);

        SetAndVerifyValues(sut, "Option A", "Option B");
        sut.SelectedIndex.Should.Equal(2);
        sut.SelectedOption.Should.Equal("Option B");
        sut.Options[2].IsSelected.Should.BeTrue();
        sut.Options[0].IsSelected.Should.BeFalse();

        VerifyDoesNotEqual(sut, "Option C");

        Assert.Throws<NoSuchElementException>(() =>
            sut.Set("Missing Value"));

        sut.Options.Should.EqualSequence("--select--", "Option A", "Option B", "Option C", "Option D");
    }

    [Test]
    public void OfStringType_WithFormatAttribute()
    {
        var sut = _page.TextSelectWithFromat;

        SetAndVerifyValues(sut, "A", "B");

        VerifyDoesNotEqual(sut, "C");

        sut.Options[1].Should.Equal("A");
        sut.Options[4].Should.Equal("D");
    }

    [Test]
    public void OfStringType_WithTermMatchContains()
    {
        var sut = _page.TextSelectWithContainsMatch;

        sut.Set("A");
        sut.Should.Equal("Option A");

        sut.Set("C");
        sut.SelectedOption.Should.Equal("Option C");
    }

    [Test]
    public void OfEnumType_ByText()
    {
        var sut = _page.EnumSelectByText;

        SetAndVerifyValues(sut, SelectPage.Option.OptionA, SelectPage.Option.OptionC);
        VerifyDoesNotEqual(sut, SelectPage.Option.OptionD);
    }

    [Test]
    public void OfEnumType_ByValue()
    {
        var sut = _page.EnumSelectByValue;

        SetAndVerifyValues(sut, SelectPage.Option.OptionB, SelectPage.Option.OptionA);
        VerifyDoesNotEqual(sut, SelectPage.Option.OptionB);
    }

    [Test]
    public void OfIntType_ByText()
    {
        var sut = _page.IntSelectByText;

        VerifyEquals(sut, 1);
        SetAndVerifyValues(sut, 4, 2);
        VerifyDoesNotEqual(sut, 3);
    }

    [Test]
    public void OfEnumType_WhenThereIsEmptyOption()
    {
        var sut = _page.EnumSelectWithEmptyOption;

        SetAndVerifyValues(sut, SelectPage.EnumWithEmptyOption.A, SelectPage.EnumWithEmptyOption.None);
        VerifyDoesNotEqual(sut, SelectPage.EnumWithEmptyOption.B);
    }

    [Test]
    public void OfEnumType_WhenNullableAndThereIsNoEmptyOption()
    {
        var sut = _page.NullableEnumSelectWithEmptyOption;

        SetAndVerifyValues(sut, SelectPage.EnumWithoutEmptyOption.A, null, SelectPage.EnumWithoutEmptyOption.B);
        VerifyDoesNotEqual(sut, SelectPage.EnumWithoutEmptyOption.C);
    }
}
