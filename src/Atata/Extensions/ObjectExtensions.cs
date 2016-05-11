namespace Atata
{
    public static class ObjectExtensions
    {
        public static string ToString(this object value, TermFormat format)
        {
            value.CheckNotNull("value");

            return TermResolver.ToString(value, new TermOptions { Format = format });
        }
    }
}
