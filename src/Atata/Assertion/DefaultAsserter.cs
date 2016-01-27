using Humanizer;
using System.Text.RegularExpressions;

namespace Atata
{
    public class DefaultAsserter : IAsserter
    {
        public void That(bool condition, string message, params object[] args)
        {
            if (!condition)
                throw new AssertionException(string.Format(message, args));
        }

        public void NotNull(object actual, string message, params object[] args)
        {
            if (object.Equals(actual, null))
                throw ExceptionsFactory.CreateForFailedAssert("not null", "null", message, args);
        }

        public void AreEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (!object.Equals(expected, actual))
                throw ExceptionsFactory.CreateForFailedAssert(expected, actual, message, args);
        }

        public void AreNotEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (object.Equals(expected, actual))
                throw ExceptionsFactory.CreateForFailedAssert("not equal to {0}".FormatWith(expected), actual, message, args);
        }

        public void ContainsSubstring(string expected, string actual, string message, params object[] args)
        {
            if (!actual.Contains(expected))
                throw ExceptionsFactory.CreateForFailedAssert("String containing '{0}'".FormatWith(expected), "'{0}'".FormatWith(actual), message, args);
        }

        public void IsMatch(string pattern, string actual, string message, params object[] args)
        {
            if (!Regex.IsMatch(actual, pattern))
                throw ExceptionsFactory.CreateForFailedAssert("String matching '{0}'".FormatWith(pattern), "'{0}'".FormatWith(actual), message, args);
        }
    }
}
