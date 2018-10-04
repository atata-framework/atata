namespace Atata.Tests
{
    using _ = DragAndDropPage;

    [Url("draganddrop")]
    public class DragAndDropPage : Page<_>
    {
        [FindById]
        public ItemsControl<DragItem, _> DropContainer { get; private set; }

        [FindById]
        public ItemsControl<DragItem, _> DragItems { get; private set; }

        [ControlDefinition("span", ContainingClass = "drag-item")]
        [DragAndDropUsingDomEvents]
        public class DragItem : Control<_>
        {
        }
    }
}
