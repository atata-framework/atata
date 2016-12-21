using System;

namespace Atata
{
    [Flags]
    public enum AttributeLevels
    {
        None = 0,

        Declared = 1 << 0,

        ParentComponent = 1 << 1,

        Assembly = 1 << 2,

        Global = 1 << 3,

        Component = 1 << 4,

        DeclaredAndComponent = Declared | Component,

        NonComponent = Declared | ParentComponent | Assembly | Global,

        All = Declared | ParentComponent | Assembly | Global | Component
    }
}
