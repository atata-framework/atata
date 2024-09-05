namespace Atata.IntegrationTests.Finding;

public sealed class FindByRelativeElementContentAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void WithValue() =>
        Go.To<FindingPage>()
           .Find<TextInput<FindingPage>>(new FindByRelativeElementContentAttribute("preceding-sibling::*[1]", "User Name"))
               .DomProperties.Id.Should.Be("user-name");

    [Test]
    public void WithValueAndIndex() =>
        Go.To<FindingPage>()
           .Find<Label<FindingPage>>(new FindByRelativeElementContentAttribute("preceding-sibling::legend", "Radio Buttons") { Index = 2 })
               .Content.Should.Be("Option C");
}
