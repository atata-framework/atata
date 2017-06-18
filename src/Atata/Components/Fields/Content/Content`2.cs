namespace Atata
{
    /// <summary>
    /// Represents any HTML element containing content. Default search is performed by the label.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "element")]
    public class Content<T, TOwner> : Field<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string DataProviderName
        {
            get { return "content"; }
        }

        protected override T GetValue()
        {
            string value = GetContent();
            return ConvertStringToValue(value);
        }
    }
}
