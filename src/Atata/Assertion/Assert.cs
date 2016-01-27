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

        // TODO: Perhaps remove ContainsSubstring method.
        internal static AssertionDelgate<string> ContainsSubstring { get; private set; }
        internal static AssertionDelgate<string> IsMatch { get; private set; }

        private static void Apply(IAsserter asserter)
        {
            That = asserter.That;
            NotNull = asserter.NotNull;
            AreEqual = asserter.AreEqual;
            AreNotEqual = asserter.AreNotEqual;
            ContainsSubstring = asserter.ContainsSubstring;
            IsMatch = asserter.IsMatch;
        }
    }
}
