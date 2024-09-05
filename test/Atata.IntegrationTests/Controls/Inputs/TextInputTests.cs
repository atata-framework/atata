namespace Atata.IntegrationTests.Controls.Inputs;

public class TextInputTests : TextBasedInputTestSuiteBase
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.TextInput;
}
