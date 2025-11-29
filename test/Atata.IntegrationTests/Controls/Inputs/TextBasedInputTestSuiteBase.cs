namespace Atata.IntegrationTests.Controls.Inputs;

public abstract class TextBasedInputTestSuiteBase : WebDriverSessionTestSuite
{
    private InputPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<InputPage>();

    protected abstract Input<string, InputPage> ResolveSut(InputPage page);

    protected Input<string, InputPage> ResolveSut() =>
        ResolveSut(_page);

    [Test]
    public void Interact()
    {
        var sut = ResolveSut();

        VerifyEquals(sut, string.Empty);

        SetAndVerifyValues(sut, "Text1", string.Empty, "Text2");

        VerifyDoesNotEqual(sut, "Text3");

        sut.Type("0");
        sut.Should.Be("Text20");
        sut.Clear();
        sut.Should.BeEmpty();

        sut.Type("1");
        sut.Set(null!);
        sut.Should.BeEmpty();
    }
}
