namespace Atata
{
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
