namespace Atata.IntegrationTests.Controls.Inputs;

public class SearchInputTests : TextBasedInputTestSuiteBase
{
    protected override Input<string, InputPage> ResolveSut(InputPage page) =>
        page.SearchInput;
}
