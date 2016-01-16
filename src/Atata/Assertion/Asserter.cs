namespace Atata
{
    public static class Asserter
    {
        static Asserter()
        {
            // TODO: Search for asserter.
            IAsserter asserter = new DefaultAsserter();
            Apply(asserter);
        }

        public delegate void AssertThatDelegate(bool condition, string message, params object[] args);
        public delegate void AssertNotNullDelegate(object actual, string message, params object[] args);
        public delegate void AssertAreEqualDelgate<T>(T expected, T actual, string message, params object[] args);
        public delegate void AssertContainsSubstringDelegate(string expected, string actual, string message, params object[] args);

        public static AssertThatDelegate That { get; private set; }
        public static AssertNotNullDelegate NotNull { get; private set; }
        public static AssertAreEqualDelgate<object> AreEqual { get; private set; }

        // TODO: Perhaps remove ContainsSubstring method.
        public static AssertContainsSubstringDelegate ContainsSubstring { get; private set; }

        private static void Apply(IAsserter asserter)
        {
            That = asserter.That;
            NotNull = asserter.NotNull;
            AreEqual = asserter.AreEqual;
            ContainsSubstring = asserter.ContainsSubstring;
        }
    }
}
