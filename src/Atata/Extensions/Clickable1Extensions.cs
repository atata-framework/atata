namespace Atata
{
    public static class Clickable1Extensions
    {
        public static TOwner VerifyEnabled<TOwner>(this _Clickable<TOwner> link)
            where TOwner : PageObject<TOwner>
        {
            return default(TOwner);
        }
    }
}
