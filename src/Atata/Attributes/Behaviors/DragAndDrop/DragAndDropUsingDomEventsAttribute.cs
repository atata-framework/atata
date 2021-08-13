using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for drag and drop using JavaScript.
    /// The script simulates drag and drop by dispatching DOM events: 'dragstart', 'dragenter', 'dragover', 'drop' and 'dragend'.
    /// </summary>
    [Obsolete("Use " + nameof(DragsAndDropsUsingDomEventsAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class DragAndDropUsingDomEventsAttribute : DragsAndDropsUsingDomEventsAttribute
    {
    }
}
