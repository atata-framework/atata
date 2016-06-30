using System;

namespace Atata
{
    /// <summary>
    /// Represents the time input control.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='time' or not(@type)]")]
    public class TimeInput<TOwner> : Input<TimeSpan?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
