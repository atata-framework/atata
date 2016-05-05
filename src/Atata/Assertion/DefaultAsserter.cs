using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Atata
{
    public class DefaultAsserter : IAsserter
    {
        private const string NullString = "null";

        public void IsTrue(bool condition, string message, params object[] args)
        {
            if (!condition)
                throw new AssertionException(string.Format(message, args));
        }

        public void IsFalse(bool condition, string message, params object[] args)
        {
            if (condition)
                throw new AssertionException(string.Format(message, args));
        }

        public void NotNull(object actual, string message, params object[] args)
        {
            if (Equals(actual, null))
                throw CreateExceptionForFailedAssert("not null", "null", message, args);
        }

        public void AreEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (!Equals(expected, actual))
                throw CreateExceptionForFailedAssert(expected, actual, message, args);
        }

        public void AreNotEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (Equals(expected, actual))
                throw CreateExceptionForFailedAssert("not equal to {0}".FormatWith(Equals(expected, null) ? "null" : expected.ToString()), actual, message, args);
        }

        public void Contains(string expected, string actual, string message, params object[] args)
        {
            if (!actual.Contains(expected))
                throw CreateExceptionForFailedAssert("String containing \"{0}\"".FormatWith(expected), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void StartsWith(string expected, string actual, string message, params object[] args)
        {
            if (!actual.StartsWith(expected))
                throw CreateExceptionForFailedAssert("String starting with \"{0}\"".FormatWith(expected), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void EndsWith(string expected, string actual, string message, params object[] args)
        {
            if (!actual.EndsWith(expected))
                throw CreateExceptionForFailedAssert("String ending with \"{0}\"".FormatWith(expected), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void IsMatch(string pattern, string actual, string message, params object[] args)
        {
            if (!Regex.IsMatch(actual, pattern))
                throw CreateExceptionForFailedAssert("String matching \"{0}\"".FormatWith(pattern), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void DoesNotContain(string expected, string actual, string message, params object[] args)
        {
            if (actual.Contains(expected))
                throw CreateExceptionForFailedAssert("String not containing \"{0}\"".FormatWith(expected), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void DoesNotStartWith(string expected, string actual, string message, params object[] args)
        {
            if (actual.StartsWith(expected))
                throw CreateExceptionForFailedAssert("String not starting with \"{0}\"".FormatWith(expected), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void DoesNotEndWith(string expected, string actual, string message, params object[] args)
        {
            if (actual.EndsWith(expected))
                throw CreateExceptionForFailedAssert("String not ending with \"{0}\"".FormatWith(expected), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void DoesNotMatch(string pattern, string actual, string message, params object[] args)
        {
            if (Regex.IsMatch(actual, pattern))
                throw CreateExceptionForFailedAssert("String not matching \"{0}\"".FormatWith(pattern), "\"{0}\"".FormatWith(actual), message, args);
        }

        public void IsSubsetOf(IEnumerable subset, IEnumerable superset, string message, params object[] args)
        {
            var castedSubset = subset.Cast<object>().ToArray();
            var castedSuperset = superset.Cast<object>().ToArray();

            if (castedSubset.Intersect(castedSuperset).Count() != castedSubset.Count())
                throw CreateExceptionForFailedAssert(
                    "subset of {0}".FormatWith(CollectionToString(castedSubset)),
                    CollectionToString(castedSuperset),
                    message,
                    args);
        }

        public void HasNoIntersection(IEnumerable collection1, IEnumerable collection2, string message, params object[] args)
        {
            var castedCollection1 = collection1.Cast<object>().ToArray();
            var castedCollection2 = collection2.Cast<object>().ToArray();

            if (castedCollection1.Intersect(castedCollection2).Any())
                throw CreateExceptionForFailedAssert(
                    "no intersection with {0}".FormatWith(CollectionToString(castedCollection1)),
                    CollectionToString(castedCollection2),
                    message,
                    args);
        }

        private string CollectionToString(IEnumerable<object> collection)
        {
            if (!collection.Any())
                return "<empty>";

            return "< {0} >".FormatWith(string.Join(", ", collection.Select(ObjectToString).ToArray()));
        }

        private string ObjectToString(object value)
        {
            if (value is string)
                return "\"{0}\"".FormatWith(value);
            else if (value is ValueType)
                return value.ToString();
            else if (value is IEnumerable)
                return CollectionToString(((IEnumerable)value).Cast<object>());
            else
                return "<{0}>".FormatWith(value.ToString());
        }

        private static AssertionException CreateExceptionForFailedAssert(object expected, object actual, string message = null, params object[] args)
        {
            string errorMesage = BuildAssertionErrorMessage(expected, actual, message, args);
            return new AssertionException(errorMesage);
        }

        public static string BuildAssertionErrorMessage(object expected, object actual, string message = null, params object[] args)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(message))
                builder.AppendFormat(message, args).AppendLine();

            return builder.
                AppendFormat("Expected: {0}", expected ?? NullString).
                AppendLine().
                AppendFormat("But was: {0}", actual ?? NullString)
                .ToString();
        }
    }
}
