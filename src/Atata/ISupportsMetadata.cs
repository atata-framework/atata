using System;

namespace Atata
{
    public interface ISupportsMetadata
    {
        UIComponentMetadata Metadata { get; set; }

        Type ComponentType { get; }
    }
}
