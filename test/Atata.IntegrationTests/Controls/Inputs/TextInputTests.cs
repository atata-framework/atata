namespace Atata.IntegrationTests.Controls.Inputs;

public class TextInputTests : BaseTextBasedInputUITestFixture
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.TextInput;
}
