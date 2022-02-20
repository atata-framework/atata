namespace Atata
{
    /// <summary>
    /// Represents any HTML element containing content.
    /// Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TContent">The type of the content.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "element")]
    public class Content<TContent, TOwner> : Field<TContent, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string ValueProviderName => "content";

        protected override TContent GetValue()
        {
            string value = GetContent();
            return ConvertStringToValueUsingGetFormat(value);
        }
    }
}
