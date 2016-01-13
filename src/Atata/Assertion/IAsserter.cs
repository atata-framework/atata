namespace Atata
{
    public interface IAsserter
    {
        void That(bool condition, string message, params object[] args);
        void NotNull(object actual, string message, params object[] args);
        void AreEqual<T>(T expected, T actual);
        void ContainsSubstring(string expected, string actual, string message, params object[] args);
    }
}
