namespace Atata.IntegrationTests;

using _ = DynamicListRemovingPage;

[Url("dynamiclistremoving")]
public sealed class DynamicListRemovingPage : Page<_>
{
    public DynamicListRemovingPage()
    {
        // Need to pre-initialize properties as they are expected to be configured before the page is navigated.
        ItemsContainer = Controls.Resolve<Control<_>>(nameof(ItemsContainer));
        ClickTarget = Controls.Resolve<Control<_>>(nameof(ClickTarget));
    }

    [ControlDefinition("div", ContainingClass = "item")]
    public ControlList<Text<_>, _> Items { get; private set; }

    [FindById("items")]
    public Control<_> ItemsContainer { get; private set; }

    [FindById("items")]
    public Control<_> ClickTarget { get; private set; }
}
