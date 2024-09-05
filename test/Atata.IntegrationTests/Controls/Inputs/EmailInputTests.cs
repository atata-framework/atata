namespace Atata.IntegrationTests.Controls.Inputs;

public class EmailInputTests : TextBasedInputTestSuiteBase
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.EmailInput;
}
