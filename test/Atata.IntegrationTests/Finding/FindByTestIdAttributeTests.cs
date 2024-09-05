namespace Atata.IntegrationTests.Finding;

public class FindByTestIdAttributeTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void WithValue()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Build();

        Go.To<FindingPage>()
            .Find<Control<FindingPage>>(new FindByTestIdAttribute("test-id-1"))
                .Content.Should.Be("test-id-element-1");
    }

    [Test]
    public void WithoutArguments()
    {
        ConfigureAtataContextWithWebDriverSession()
            .Build();

        Go.To<FindingPage>()
            .Find<Control<FindingPage>>("TestId1", new FindByTestIdAttribute())
                .Content.Should.Be("test-id-element-1");
    }

    [Test]
    public void WhenDomTestIdAttributePropertiesAreConfigured()
    {
        BuildAtataContextWithWebDriverSession(x => x
            .UseDomTestIdAttributeName("data-autoid")
            .UseDomTestIdAttributeDefaultCase(TermCase.PascalKebab));

        Go.To<FindingPage>()
            .Find<Control<FindingPage>>("TestId2", new FindByTestIdAttribute())
                .Content.Should.Be("test-id-element-2");
    }
}
