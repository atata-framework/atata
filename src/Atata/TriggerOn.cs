using System;

namespace Atata
{
    [Flags]
    public enum TriggerOn
    {
        Before = 1,
        After = 2,
        BeforeAndAfter = 3
    }
}
