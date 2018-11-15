using System;

namespace Atata
{
    /// <summary>
    /// Represents the time input control.
    /// Default search is performed by the label.
    /// Handles any input element with <c>type="time"</c>, <c>type="text"</c> or without the defined <c>type</c> attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='time' or not(@type)]")]
    public class TimeInput<TOwner> : Input<TimeSpan?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
