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

        public void IsTrue(bool? actual, string message, params object[] args)
        {
            if (actual != true)
                throw CreateException(bool.TrueString, ObjectToString(actual), message, args);
        }

        public void IsFalse(bool? actual, string message, params object[] args)
        {
            if (actual != false)
                throw CreateException(bool.FalseString, ObjectToString(actual), message, args);
        }

        public void IsNull(object actual, string message, params object[] args)
        {
            if (!Equals(actual, null))
                throw CreateException(NullString, ObjectToString(actual), message, args);
        }

        public void IsNotNull(object actual, string message, params object[] args)
        {
            if (Equals(actual, null))
                throw CreateException("not null", NullString, message, args);
        }

        public void IsNullOrEmpty(string actual, string message, params object[] args)
        {
            if (!string.IsNullOrEmpty(actual))
                throw CreateException("null or empty", ObjectToString(actual), message, args);
        }

        public void IsNotNullOrEmpty(string actual, string message, params object[] args)
        {
            if (string.IsNullOrEmpty(actual))
                throw CreateException("not null or empty", ObjectToString(actual), message, args);
        }

        public void AreEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (!Equals(expected, actual))
                throw CreateException(ObjectToString(expected), ObjectToString(actual), message, args);
        }

        public void AreNotEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (Equals(expected, actual))
                throw CreateException("not equal to {0}".FormatWith(ObjectToString(expected)), ObjectToString(actual), message, args);
        }

        public void Contains(string expected, string actual, string message, params object[] args)
        {
            if (!actual.Contains(expected))
                throw CreateException("String containing {0}".FormatWith(ObjectToString(expected)), ObjectToString(actual), message, args);
        }

        public void StartsWith(string expected, string actual, string message, params object[] args)
        {
            if (!actual.StartsWith(expected))
                throw CreateException("String starting with {0}".FormatWith(ObjectToString(expected)), ObjectToString(actual), message, args);
        }

        public void EndsWith(string expected, string actual, string message, params object[] args)
        {
            if (!actual.EndsWith(expected))
                throw CreateException("String ending with {0}".FormatWith(ObjectToString(expected)), ObjectToString(actual), message, args);
        }

        public void IsMatch(string pattern, string actual, string message, params object[] args)
        {
            if (!Regex.IsMatch(actual, pattern))
                throw CreateException("String matching {0}".FormatWith(ObjectToString(pattern)), ObjectToString(actual), message, args);
        }

        public void DoesNotContain(string expected, string actual, string message, params object[] args)
        {
            if (actual.Contains(expected))
                throw CreateException("String not containing {0}".FormatWith(ObjectToString(expected)), ObjectToString(actual), message, args);
        }

        public void DoesNotStartWith(string expected, string actual, string message, params object[] args)
        {
            if (actual.StartsWith(expected))
                throw CreateException("String not starting with {0}".FormatWith(ObjectToString(expected)), ObjectToString(actual), message, args);
        }

        public void DoesNotEndWith(string expected, string actual, string message, params object[] args)
        {
            if (actual.EndsWith(expected))
                throw CreateException("String not ending with {0}".FormatWith(ObjectToString(expected)), ObjectToString(actual), message, args);
        }

        public void DoesNotMatch(string pattern, string actual, string message, params object[] args)
        {
            if (Regex.IsMatch(actual, pattern))
                throw CreateException("String not matching {0}".FormatWith(ObjectToString(pattern)), ObjectToString(actual), message, args);
        }

        public void IsSubsetOf(IEnumerable subset, IEnumerable superset, string message, params object[] args)
        {
            var castedSubset = subset.Cast<object>().ToArray();
            var castedSuperset = superset.Cast<object>().ToArray();

            if (castedSubset.Intersect(castedSuperset).Count() != castedSubset.Count())
                throw CreateException(
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
                throw CreateException(
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
            if (Equals(value, null))
                return NullString;
            else if (value is string)
                return "\"{0}\"".FormatWith(value);
            else if (value is ValueType)
                return value.ToString();
            else if (value is IEnumerable)
                return CollectionToString(((IEnumerable)value).Cast<object>());
            else
                return "{{{0}}}".FormatWith(value.ToString());
        }

        private static AssertionException CreateException(object expected, object actual, string message = null, params object[] args)
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
                AppendFormat("But was: {0}", actual ?? NullString).
                ToString();
        }
    }
}
