using System;

namespace Atata
{
    [Flags]
    public enum TriggerEvents
    {
        None = 0,
        BeforeClick = 1 << 0,
        AfterClick = 1 << 1,
        BeforeGet = 1 << 2,
        AfterGet = 1 << 3,
        BeforeSet = 1 << 4,
        AfterSet = 1 << 5,
        OnPageObjectInit = 1 << 6,
        OnPageObjectLeave = 1 << 7,
        BeforeAndAfterClick = BeforeClick | AfterClick,
        BeforeGetOrSet = BeforeGet | BeforeSet,
        BeforeAnyAction = BeforeClick | BeforeGet | BeforeSet,
        AfterGetOrSet = AfterGet | AfterSet,
        AfterAnyAction = AfterClick | AfterGet | AfterSet,
        AfterClickOrSet = AfterClick | AfterSet
    }
}
