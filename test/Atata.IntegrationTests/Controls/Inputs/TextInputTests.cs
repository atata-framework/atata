namespace Atata.IntegrationTests.Controls.Inputs;

public sealed class TextInputTests : TextBasedInputTestSuiteBase
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.TextInput;

    [Test]
    public void TypeRandom()
    {
        var sut = ResolveSut();

        sut.TypeRandom(out string value1);
        sut.TypeRandom(out string value2);

        value1.Should().NotBeNullOrEmpty();
        value2.Should().NotBeNullOrEmpty();
        sut.Should.Be(value1 + value2);
    }
}
