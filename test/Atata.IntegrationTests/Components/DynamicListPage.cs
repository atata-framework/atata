namespace Atata.IntegrationTests;

using _ = DynamicListPage;

[Url("dynamiclist")]
public class DynamicListPage : Page<_>
{
    [ControlDefinition("div", ContainingClass = "item")]
    public ControlList<Text<_>, _> Items { get; private set; }
}
