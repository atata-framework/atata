using System;

namespace Atata
{
    [Flags]
    public enum TriggerEvents
    {
        None = 0,

        OnPageObjectInit = 1 << 0,
        OnPageObjectLeave = 1 << 1,

        BeforeClick = 1 << 2,
        AfterClick = 1 << 3,

        BeforeGet = 1 << 4,
        AfterGet = 1 << 5,

        BeforeSet = 1 << 6,
        AfterSet = 1 << 7,

        BeforeFocus = 1 << 8,
        AfterFocus = 1 << 9,

        BeforeAndAfterClick = BeforeClick | AfterClick,
        BeforeGetOrSet = BeforeGet | BeforeSet,
        BeforeClickOrFocus = BeforeClick | BeforeFocus,
        BeforeAnyAction = BeforeClick | BeforeGet | BeforeSet | BeforeFocus,

        AfterGetOrSet = AfterGet | AfterSet,
        AfterClickOrFocus = AfterClick | AfterFocus,
        AfterAnyAction = AfterClick | AfterGet | AfterSet | AfterFocus,
        AfterClickOrSet = AfterClick | AfterSet,
    }
}
