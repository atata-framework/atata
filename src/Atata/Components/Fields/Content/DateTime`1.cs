using System;

namespace Atata
{
    /// <summary>
    /// Represents any element containing date and time content.
    /// Default search finds the first occurring element.
    /// The default format is "g" (general date/time pattern (short time), e.g. 6/15/2009 1:45 PM).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [Format("g")]
    public class DateTime<TOwner> : Content<DateTime?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
