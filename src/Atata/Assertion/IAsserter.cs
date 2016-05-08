using System;
using System.Collections;

namespace Atata
{
    public interface IAsserter
    {
        void IsTrue(bool? condition, string message, params object[] args);
        void IsFalse(bool? condition, string message, params object[] args);
        void IsNull(object actual, string message, params object[] args);
        void IsNotNull(object actual, string message, params object[] args);
        void IsNullOrEmpty(string actual, string message, params object[] args);
        void IsNotNullOrEmpty(string actual, string message, params object[] args);
        void AreEqual<T>(T expected, T actual, string message, params object[] args);
        void AreNotEqual<T>(T expected, T actual, string message, params object[] args);

        void Greater(IComparable expected, IComparable actual, string message, params object[] args);
        void GreaterOrEqual(IComparable value1, IComparable value2, string message, params object[] args);
        void Less(IComparable value1, IComparable value2, string message, params object[] args);
        void LessOrEqual(IComparable value1, IComparable value2, string message, params object[] args);
        void IsInRange(IComparable from, IComparable to, IComparable actual, string message, params object[] args);

        void AreEqualIgnoringCase(string expected, string actual, string message, params object[] args);
        void AreNotEqualIgnoringCase(string expected, string actual, string message, params object[] args);
        void Contains(string expected, string actual, string message, params object[] args);
        void StartsWith(string expected, string actual, string message, params object[] args);
        void EndsWith(string expected, string actual, string message, params object[] args);
        void IsMatch(string pattern, string actual, string message, params object[] args);
        void DoesNotContain(string expected, string actual, string message, params object[] args);
        void DoesNotStartWith(string expected, string actual, string message, params object[] args);
        void DoesNotEndWith(string expected, string actual, string message, params object[] args);
        void DoesNotMatch(string pattern, string actual, string message, params object[] args);
        void IsSubsetOf(IEnumerable subset, IEnumerable superset, string message, params object[] args);
        void HasNoIntersection(IEnumerable collection1, IEnumerable collection2, string message, params object[] args);
    }
}
