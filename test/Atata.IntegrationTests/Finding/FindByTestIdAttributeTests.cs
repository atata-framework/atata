namespace Atata.IntegrationTests.Finding;

public class FindByTestIdAttributeTests : UITestFixtureBase
{
    [Test]
    public void WithValue()
    {
        ConfigureBaseAtataContext()
            .Build();

        Go.To<FindingPage>()
            .Find<Control<FindingPage>>(new FindByTestIdAttribute("test-id-1"))
                .Content.Should.Be("test-id-element-1");
    }

    [Test]
    public void WithoutArguments()
    {
        ConfigureBaseAtataContext()
            .Build();

        Go.To<FindingPage>()
            .Find<Control<FindingPage>>("TestId1", new FindByTestIdAttribute())
                .Content.Should.Be("test-id-element-1");
    }

    [Test]
    public void WhenDomTestIdAttributePropertiesAreConfigured()
    {
        ConfigureBaseAtataContext()
            .UseDomTestIdAttributeName("data-autoid")
            .UseDomTestIdAttributeDefaultCase(TermCase.PascalKebab)
            .Build();

        Go.To<FindingPage>()
            .Find<Control<FindingPage>>("TestId2", new FindByTestIdAttribute())
                .Content.Should.Be("test-id-element-2");
    }
}
