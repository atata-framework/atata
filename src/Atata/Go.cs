namespace Atata
{
    public static class Go
    {
        public static T To<T>(T pageObject = null, string url = null, bool preserveState = false)
            where T : PageObject<T>
        {
            return default(T);
        }
    }
}
