namespace Atata
{
    public static class LinkDelegate1Extensions
    {
        public static Link<TOwner> GetControl<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return (Link<TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner Click<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Click();
        }

        public static TOwner Hover<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Hover();
        }

        public static TOwner Focus<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Focus();
        }

        public static TOwner DoubleClick<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().DoubleClick();
        }

        public static TOwner RightClick<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().RightClick();
        }

        public static TOwner ScrollTo<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().ScrollTo();
        }

        public static bool IsEnabled<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled;
        }

        public static bool Exists<TOwner>(this LinkDelegate<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TOwner>(this LinkDelegate<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Missing(options);
        }

        public static ValueProvider<string, TOwner> Content<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Should;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> ExpectTo<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().ExpectTo;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> WaitTo<TOwner>(this LinkDelegate<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().WaitTo;
        }
    }
}
