namespace Atata
{
    public class Select<TOwner> : SelectBase<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string GetValue()
        {
            return GetSelectedOptionValue();
        }

        protected override void SetValue(string value)
        {
            SetSelectedOptionValue(value);
        }
    }
}
