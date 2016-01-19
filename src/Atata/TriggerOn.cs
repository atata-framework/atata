using System;

namespace Atata
{
    [Flags]
    public enum TriggerOn
    {
        None = 0,
        Before = 1,
        After = 2,
        BeforeAndAfter = 3
    }
}
