using System;

namespace Atata
{
    [Obsolete("Do not use this interface as it will be removed.")] // Obsolete since v1.1.0.
    public interface IItemsControl
    {
        void Apply(IItemElementFindStrategy itemElementFindStrategy);
    }
}
