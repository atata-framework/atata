namespace Atata
{
    public static class Link2Extensions
    {
        public static TOwner VerifyEnabled<TNavigateTo, TOwner>(this _Link<TNavigateTo, TOwner> link)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return default(TOwner);
        }
    }
}
