#nullable enable

namespace Atata;

public interface ITermFindAttribute
{
    string[] GetTerms(UIComponentMetadata metadata);
}
