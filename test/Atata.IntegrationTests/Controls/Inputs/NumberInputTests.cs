namespace Atata.IntegrationTests.Controls.Inputs;

public sealed class NumberInputTests : WebDriverSessionTestSuite
{
    private InputPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<InputPage>();

    [Test]
    public void Interact()
    {
        var sut = _page.NumberInput;

        VerifyEquals(sut, null);

        SetAndVerifyValues(sut, 45, null, 57);

        VerifyDoesNotEqual(sut, 59);

        sut.Should.BeGreater(55);
        sut.Should.BeLess(60);
        sut.Should.BeInRange(50, 60);
        sut.Get(out int? intNumber);

        Assert.That(intNumber, Is.EqualTo(57));

        sut.SetRandom(out intNumber);
        sut.Should.Be(intNumber);
    }

    [Test]
    public void TypeRandom()
    {
        var sut = _page.NumberInput;

        sut.TypeRandom(out decimal value1);
        sut.TypeRandom(out decimal value2);

        sut.Should.Be(decimal.Parse(value1.ToString() + value2.ToString()));
    }
}
