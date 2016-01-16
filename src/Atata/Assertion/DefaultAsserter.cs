using System.Text;

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
                throw new AssertionException(FormatExceptionMessage("not null", "null", message, args));
        }

        public void AreEqual<T>(T expected, T actual, string message, params object[] args)
        {
            if (!object.Equals(expected, actual))
                throw new AssertionException(FormatExceptionMessage(expected, actual, message, args));
        }

        public void ContainsSubstring(string expected, string actual, string message, params object[] args)
        {
            if (!actual.Contains(expected))
            {
                StringBuilder builder = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(message))
                    builder.AppendFormat(message, args).AppendLine();

                builder.
                    AppendFormat("Expected: String containing '{0}'", expected).
                    AppendLine().
                    AppendFormat("But was: '{0}'", actual);
                throw new AssertionException(builder.ToString());
            }
        }

        private static string FormatExceptionMessage(object expected, object actual, string message = null, params object[] args)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(message))
                builder.AppendFormat(message, args).AppendLine();

            return builder.
                AppendFormat("Expected: {0}", expected).
                AppendLine().
                AppendFormat("But was: {0}", actual)
                .ToString();
        }
    }
}
