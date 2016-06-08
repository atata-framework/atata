namespace Atata
{
    /// <summary>
    /// Represents any element containing content.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Content<T, TOwner> : Field<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string ValueProviderName
        {
            get { return "content"; }
        }

        protected override T GetValue()
        {
            string value = Scope.Text;
            return ConvertStringToValue(value);
        }
    }
}
