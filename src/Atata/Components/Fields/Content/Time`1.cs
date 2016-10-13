using System;

namespace Atata
{
    /// <summary>
    /// Represents any element containing time content. Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Time<TOwner> : Content<TimeSpan?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
