using System.Linq;

namespace Atata
{
    public static class ITermDataProviderExtensions
    {
        public static string[] GetActualValues(this ITermDataProvider provider, string fallbackValue)
        {
            string[] rawValues = (provider.Values != null && provider.Values.Any()) ? provider.Values : new[] { provider.Case.ApplyTo(fallbackValue) };

            return !string.IsNullOrEmpty(provider.Format) ? rawValues.Select(x => string.Format(provider.Format, x)).ToArray() : rawValues;
        }
    }
}
