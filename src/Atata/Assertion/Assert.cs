using System.Collections;

namespace Atata
{
    internal static class Assert
    {
        static Assert()
        {
            // TODO: Search for asserter.
            IAsserter asserter = new DefaultAsserter();
            Apply(asserter);
        }

        public delegate void AssertionDelgate<T>(T expected, T actual, string message, params object[] args);
        public delegate void AssertionSimpleDelegate<T>(T actual, string message, params object[] args);

        internal static AssertionSimpleDelegate<bool> That { get; private set; }
        internal static AssertionSimpleDelegate<object> NotNull { get; private set; }
        internal static AssertionDelgate<object> AreEqual { get; private set; }
        internal static AssertionDelgate<object> AreNotEqual { get; private set; }

        internal static AssertionDelgate<string> Contains { get; private set; }
        internal static AssertionDelgate<string> StartsWith { get; private set; }
        internal static AssertionDelgate<string> EndsWith { get; private set; }
        internal static AssertionDelgate<string> IsMatch { get; private set; }
        internal static AssertionDelgate<string> DoesNotContain { get; private set; }
        internal static AssertionDelgate<string> DoesNotStartWith { get; private set; }
        internal static AssertionDelgate<string> DoesNotEndWith { get; private set; }
        internal static AssertionDelgate<string> DoesNotMatch { get; private set; }
        internal static AssertionDelgate<IEnumerable> IsSubsetOf { get; private set; }
        internal static AssertionDelgate<IEnumerable> HasNoIntersection { get; private set; }

        private static void Apply(IAsserter asserter)
        {
            That = asserter.That;
            NotNull = asserter.NotNull;
            AreEqual = asserter.AreEqual;
            AreNotEqual = asserter.AreNotEqual;
            Contains = asserter.Contains;
            StartsWith = asserter.StartsWith;
            EndsWith = asserter.EndsWith;
            IsMatch = asserter.IsMatch;
            DoesNotContain = asserter.DoesNotContain;
            DoesNotStartWith = asserter.DoesNotStartWith;
            DoesNotEndWith = asserter.DoesNotEndWith;
            DoesNotMatch = asserter.DoesNotMatch;
            IsSubsetOf = asserter.IsSubsetOf;
            HasNoIntersection = asserter.HasNoIntersection;
        }
    }
}
