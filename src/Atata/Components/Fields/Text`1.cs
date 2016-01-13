namespace Atata
{
    [UIComponent("*")]
    public class Text<TOwner> : Field<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string GetValue()
        {
            return Scope.Text;
        }
    }
}
