namespace Atata.IntegrationTests.Controls.Inputs;

public class EmailInputTests : BaseTextBasedInputUITestFixture
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.EmailInput;
}
