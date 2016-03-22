using Humanizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                throw ExceptionFactory.CreateForFailedAssert("not null", "null", message, args);
        }

        public void AreEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (!object.Equals(expected, actual))
                throw ExceptionFactory.CreateForFailedAssert(expected, actual, message, args);
        }

        public void AreNotEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (object.Equals(expected, actual))
                throw ExceptionFactory.CreateForFailedAssert("not equal to {0}".FormatWith(expected), actual, message, args);
        }

        public void ContainsSubstring(string expected, string actual, string message, params object[] args)
        {
            if (!actual.Contains(expected))
                throw ExceptionFactory.CreateForFailedAssert("String containing '{0}'".FormatWith(expected), "'{0}'".FormatWith(actual), message, args);
        }

        public void IsMatch(string pattern, string actual, string message, params object[] args)
        {
            if (!Regex.IsMatch(actual, pattern))
                throw ExceptionFactory.CreateForFailedAssert("String matching '{0}'".FormatWith(pattern), "'{0}'".FormatWith(actual), message, args);
        }

        public void IsSubsetOf(IEnumerable subset, IEnumerable superset, string message, params object[] args)
        {
            var castedSubset = subset.Cast<object>().ToArray();
            var castedSuperset = superset.Cast<object>().ToArray();

            if (castedSubset.Intersect(castedSuperset).Count() != castedSubset.Count())
                throw ExceptionFactory.CreateForFailedAssert(
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
                throw ExceptionFactory.CreateForFailedAssert(
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
    }
}
