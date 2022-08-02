namespace Atata.IntegrationTests.Controls.Inputs;

public class SearchInputTests : BaseTextBasedInputUITestFixture
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.SearchInput;
}
