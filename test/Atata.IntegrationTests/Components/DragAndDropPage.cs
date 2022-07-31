namespace Atata.IntegrationTests;

using _ = DragAndDropPage;

[Url("actions/draganddrop")]
public class DragAndDropPage : Page<_>
{
    [FindById]
    public ItemsControl<DragItem, _> DropContainer { get; private set; }

    [FindById]
    public ItemsControl<DragItem, _> DragItems { get; private set; }

    [ControlDefinition("span", ContainingClass = "drag-item")]
    [DragsAndDropsUsingDomEvents]
    public class DragItem : Control<_>
    {
    }
}
