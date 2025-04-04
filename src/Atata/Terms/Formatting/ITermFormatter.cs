#nullable enable

namespace Atata;

public interface ITermFormatter
{
    string Format(string[] words, CultureInfo culture);
}
