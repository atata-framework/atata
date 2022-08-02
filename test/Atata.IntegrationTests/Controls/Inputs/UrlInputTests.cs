namespace Atata.IntegrationTests.Controls.Inputs;

public class UrlInputTests : BaseTextBasedInputUITestFixture
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.UrlInput;
}
