namespace Atata
{
    public static class TermCaseExtensions
    {
        public static string ApplyTo(this TermCase termCase, string value)
        {
            return TermCaseResolver.ApplyCase(value, termCase);
        }
    }
}
