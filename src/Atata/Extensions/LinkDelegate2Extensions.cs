namespace Atata
{
    public static class LinkDelegate2Extensions
    {
        public static Link<TNavigateTo, TOwner> GetControl<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return (Link<TNavigateTo, TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner Click<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Click();
        }

        public static TOwner Hover<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Hover();
        }

        public static TOwner Focus<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Focus();
        }

        public static TOwner DoubleClick<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().DoubleClick();
        }

        public static TOwner RightClick<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().RightClick();
        }

        public static TOwner ScrollTo<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().ScrollTo();
        }

        public static bool IsEnabled<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled;
        }

        public static bool Exists<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Missing(options);
        }

        public static ValueProvider<string, TOwner> Content<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Should;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> ExpectTo<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().ExpectTo;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> WaitTo<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().WaitTo;
        }
    }
}
