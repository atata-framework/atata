using System;

namespace Atata
{
    /// <summary>
    /// Represents any element containing time content.
    /// Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Time<TOwner> : Content<TimeSpan?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
