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

        public static TOwner VerifyDoesNotContain<TOwner>(this Field<string, TOwner> field, string value)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.DoesNotContain(value, actual, "Invalid {0} value", field.ComponentFullName), "does not contain \"{0}\"", value);
        }

        public static TOwner VerifyDoesNotStartWith<TOwner>(this Field<string, TOwner> field, string value)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.DoesNotStartWith(value, actual, "Invalid {0} value", field.ComponentFullName), "does not start with \"{0}\"", value);
        }

        public static TOwner VerifyDoesNotEndWith<TOwner>(this Field<string, TOwner> field, string value)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.DoesNotEndWith(value, actual, "Invalid {0} value", field.ComponentFullName), "does not end with \"{0}\"", value);
        }

        public static TOwner VerifyDoesNotMatch<TOwner>(this Field<string, TOwner> field, string pattern)
            where TOwner : PageObject<TOwner>
        {
            return field.Verify(actual => Assert.DoesNotMatch(pattern, actual, "Invalid {0} value", field.ComponentFullName), "does not match pattern \"{0}\"", pattern);
        }
    }
}
