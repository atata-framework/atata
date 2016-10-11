using System;

namespace Atata
{
    /// <summary>
    /// Represents the date input control. Default search is performed by the label. The default format is "d" (short date pattern, e.g. 6/15/2009). Handles any input element with type="date", type="text" or without the defined type attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='date' or not(@type)]")]
    [Format("d")]
    public class DateInput<TOwner> : Input<DateTime?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
