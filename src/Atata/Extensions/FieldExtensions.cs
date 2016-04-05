namespace Atata
{
    public static class FieldExtensions
    {
        public static TOwner VerifyContains<TOwner>(this Field<string, TOwner> field, string value)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.Contains(value, actual, "Invalid {0} value", field.ComponentFullName), "contains \"{0}\"", value);
        }

        public static TOwner VerifyStartsWith<TOwner>(this Field<string, TOwner> field, string value)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.StartsWith(value, actual, "Invalid {0} value", field.ComponentFullName), "starts with \"{0}\"", value);
        }

        public static TOwner VerifyEndsWith<TOwner>(this Field<string, TOwner> field, string value)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.EndsWith(value, actual, "Invalid {0} value", field.ComponentFullName), "ends with \"{0}\"", value);
        }

        public static TOwner VerifyMatches<TOwner>(this Field<string, TOwner> field, string pattern)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.IsMatch(pattern, actual, "Invalid {0} value", field.ComponentFullName), "matches pattern \"{0}\"", pattern);
        }
    }
}
