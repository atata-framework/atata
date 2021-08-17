using System.Drawing;

namespace Atata
{
    /// <summary>
    /// Represents the base behavior class for implementation of control drag and drop to offset.
    /// </summary>
    public abstract class DragAndDropToOffsetBehaviorAttribute : MulticastAttribute
    {
        public abstract void Execute<TOwner>(IUIComponent<TOwner> component, Point offset)
            where TOwner : PageObject<TOwner>;
    }
}
