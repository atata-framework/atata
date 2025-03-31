#nullable enable

namespace Atata;

internal static class Int32Extensions
{
    internal static string Ordinalize(this int number)
    {
        string ending = "th";

        int tensDigit = (number % 100) / 10;

        if (tensDigit != 1)
        {
            int unitDigit = number % 10;

            ending = unitDigit == 1 ? "st"
                : unitDigit == 2 ? "nd"
                : unitDigit == 3 ? "rd"
                : ending;
        }

        return $"{number}{ending}";
    }
}
