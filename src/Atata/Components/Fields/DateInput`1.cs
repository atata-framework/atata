using System;

namespace Atata
{
    /// <summary>
    /// Represents the date input control.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='date' or not(@type)]")]
    [Format("d")]
    public class DateInput<TOwner> : Input<DateTime?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
