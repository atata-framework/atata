namespace Atata.IntegrationTests;

using _ = DynamicListRemovingPage;

[Url("dynamiclistremoving")]
public sealed class DynamicListRemovingPage : Page<_>
{
    [ControlDefinition("div", ContainingClass = "item")]
    public ControlList<Text<_>, _> Items { get; private set; }
}
