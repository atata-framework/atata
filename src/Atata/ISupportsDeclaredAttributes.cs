using System;
using System.Collections.Generic;

namespace Atata
{
    public interface ISupportsDeclaredAttributes
    {
        List<Attribute> DeclaredAttributes { get; }
    }
}
