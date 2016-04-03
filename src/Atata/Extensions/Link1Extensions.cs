namespace Atata
{
    public static class Link1Extensions
    {
        public static TOwner VerifyEnabled<TOwner>(this _Link<TOwner> link)
            where TOwner : PageObject<TOwner>
        {
            return default(TOwner);
        }
    }
}
