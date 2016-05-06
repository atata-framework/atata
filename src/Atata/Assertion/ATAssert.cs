using System.Collections;

namespace Atata
{
    public static class ATAssert
    {
        static ATAssert()
        {
            IAsserter asserter = new DefaultAsserter();
            Apply(asserter);
        }

        public delegate void AssertionDelgate<T>(T expected, T actual, string message, params object[] args);
        public delegate void AssertionSimpleDelegate<T>(T actual, string message, params object[] args);

        public static AssertionSimpleDelegate<bool?> IsTrue { get; set; }
        public static AssertionSimpleDelegate<bool?> IsFalse { get; set; }
        public static AssertionSimpleDelegate<object> IsNull { get; set; }
        public static AssertionSimpleDelegate<object> IsNotNull { get; set; }
        public static AssertionSimpleDelegate<string> IsNullOrEmpty { get; set; }
        public static AssertionSimpleDelegate<string> IsNotNullOrEmpty { get; set; }
        public static AssertionDelgate<object> AreEqual { get; set; }
        public static AssertionDelgate<object> AreNotEqual { get; set; }

        public static AssertionDelgate<string> Contains { get; set; }
        public static AssertionDelgate<string> StartsWith { get; set; }
        public static AssertionDelgate<string> EndsWith { get; set; }
        public static AssertionDelgate<string> IsMatch { get; set; }
        public static AssertionDelgate<string> DoesNotContain { get; set; }
        public static AssertionDelgate<string> DoesNotStartWith { get; set; }
        public static AssertionDelgate<string> DoesNotEndWith { get; set; }
        public static AssertionDelgate<string> DoesNotMatch { get; set; }
        public static AssertionDelgate<IEnumerable> IsSubsetOf { get; set; }
        public static AssertionDelgate<IEnumerable> HasNoIntersection { get; set; }

        public static void Apply(IAsserter asserter)
        {
            IsTrue = asserter.IsTrue;
            IsFalse = asserter.IsFalse;
            IsNull = asserter.IsNull;
            IsNotNull = asserter.IsNotNull;
            IsNullOrEmpty = asserter.IsNullOrEmpty;
            IsNotNullOrEmpty = asserter.IsNotNullOrEmpty;
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
