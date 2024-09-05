namespace Atata.IntegrationTests.Controls.Inputs;

public class UrlInputTests : TextBasedInputTestSuiteBase
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.UrlInput;
}
