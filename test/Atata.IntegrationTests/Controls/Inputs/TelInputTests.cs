namespace Atata.IntegrationTests.Controls.Inputs;

public class TelInputTests : WebDriverSessionTestSuite
{
    private InputPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<InputPage>();

    [Test]
    public void Interact()
    {
        var sut = _page.TelInput;

        VerifyEquals(sut, string.Empty);

        SetAndVerifyValues(sut, "152-154-1456", string.Empty, "+11521541456");

        VerifyDoesNotEqual(sut, "2345325523");

        sut.Clear();
        sut.Should.BeEmpty();
    }
}
