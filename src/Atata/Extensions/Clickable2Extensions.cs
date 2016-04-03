namespace Atata
{
    public static class Clickable2Extensions
    {
        public static TOwner VerifyEnabled<TNavigateTo, TOwner>(this _Clickable<TNavigateTo, TOwner> link)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return default(TOwner);
        }
    }
}
