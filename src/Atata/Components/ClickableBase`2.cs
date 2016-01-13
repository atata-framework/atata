namespace Atata
{
    [UIComponent("*[self::input[@type='button' or @type='submit' or @type='reset'] or not(self::input)]")]
    public abstract class ClickableBase<TNavigateTo, TOwner> : Control<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        protected ClickableBase()
        {
        }

        public new TNavigateTo Click()
        {
            base.Click();
            return GetResult();
        }

        protected abstract TNavigateTo GetResult();
    }
}
