using System;

namespace Atata
{
    /// <summary>
    /// Represents the date input control.
    /// Default search is performed by the label.
    /// The default format is <c>"d"</c> (short date pattern, e.g. <c>6/15/2009</c>).
    /// Handles any <c>input</c> element with <c>type="date"</c>, <c>type="text"</c> or without the defined <c>type</c> attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='date' or not(@type)]", ComponentTypeName = "date input")]
    [Format("d")]
    public class DateInput<TOwner> : Input<DateTime?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
