#nullable enable

namespace Atata;

public interface ITermMatchFindAttribute
{
    TermMatch GetTermMatch(UIComponentMetadata metadata);
}
