namespace Atata.IntegrationTests.Controls.Inputs;

public class InputTests : WebDriverSessionTestSuite
{
    private InputPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<InputPage>();

    [Test]
    public void OfEnumType()
    {
        var sut = _page.EnumTextInput;

        SetAndVerifyValues(sut, InputPage.Option.OptionA, InputPage.Option.OptionC);

        VerifyDoesNotEqual(sut, InputPage.Option.OptionD);
    }

    [Test]
    public void OfNullableEnumType()
    {
        var sut = _page.NullableEnumTextInput;

        VerifyEquals(sut, null);

        SetAndVerifyValues(sut, InputPage.Option.OptionD, InputPage.Option.OptionA);

        VerifyDoesNotEqual(sut, InputPage.Option.OptionB);
    }

    [Test]
    public void OfIntType()
    {
        var sut = _page.IntTextInput;

        VerifyEquals(sut, null);

        SetAndVerifyValues(sut, 45, null, 57);

        VerifyDoesNotEqual(sut, 59);

        sut.Should.BeGreater(55);
        sut.Should.BeLess(60);
        sut.Should.BeInRange(50, 60);
    }
}
