namespace Atata.IntegrationTests.Finding;

public sealed class FindByPrecedingSiblingContentAttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void WithValue() =>
        Go.To<FindingPage>()
           .Find<TextInput<FindingPage>>(new FindByPrecedingSiblingContentAttribute("User Name"))
               .DomProperties.Id.Should.Be("user-name");

    [Test]
    public void WithValueAndIndex() =>
        Go.To<FindingPage>()
           .Find<Label<FindingPage>>(new FindByPrecedingSiblingContentAttribute("Radio Buttons") { Index = 2 })
               .Content.Should.Be("Option C");

    [Test]
    public void WithValueAndSiblingXPath() =>
        Go.To<FindingPage>()
           .Find<Label<FindingPage>>(new FindByPrecedingSiblingContentAttribute("Radio Buttons") { SiblingXPath = "legend[1]" })
               .Content.Should.Be("Option A");
}
