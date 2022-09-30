namespace Atata
{
    public class DragAndDropToOffsetLogSection : UIComponentLogSection
    {
        public DragAndDropToOffsetLogSection(UIComponent component, int offsetX, int offsetY)
            : base(component)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;

            Message = $"Drag and drop {component.ComponentFullName} to offset ({offsetX}, {offsetY})";
        }

        public int OffsetX { get; }

        public int OffsetY { get; }
    }
}
