namespace Atata
{
    /// <summary>
    /// Represents the select control (&lt;select&gt;). Selects an option using string. Property can be marked with <see cref="SelectByValueAttribute"/> or <see cref="SelectByTextAttribute"/>. By default selects by text.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Select<TOwner> : Select<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
