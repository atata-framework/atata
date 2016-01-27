namespace Atata
{
    public static class FieldExtensions
    {
        public static TOwner VerifyMatches<TOwner>(this Field<string, TOwner> field, string pattern)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.IsMatch(pattern, actual, "Invalid {0} value", field.ComponentName), "matches pattern '{0}'", pattern);
        }
    }
}
