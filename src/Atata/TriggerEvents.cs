using System;

namespace Atata
{
    [Flags]
    public enum TriggerEvents
    {
        None = 0,

        OnPageObjectInit = 1 << 0,
        OnPageObjectLeave = 1 << 1,

        BeforeGet = 1 << 2,
        AfterGet = 1 << 3,

        BeforeSet = 1 << 4,
        AfterSet = 1 << 5,

        BeforeClick = 1 << 6,
        AfterClick = 1 << 7,

        BeforeHover = 1 << 8,
        AfterHover = 1 << 9,

        BeforeFocus = 1 << 10,
        AfterFocus = 1 << 11,

        BeforeGetOrSet = BeforeGet | BeforeSet,
        BeforeClickOrHover = BeforeClick | BeforeHover,
        BeforeClickOrFocus = BeforeClick | BeforeFocus,
        BeforeClickOrHoverOrFocus = BeforeClick | BeforeHover | BeforeFocus,
        BeforeAnyAction = BeforeClick | BeforeGet | BeforeSet | BeforeFocus,

        AfterGetOrSet = AfterGet | AfterSet,
        AfterClickOrHover = AfterClick | AfterHover,
        AfterClickOrFocus = AfterClick | AfterFocus,
        AfterClickOrHoverOrFocus = AfterClick | AfterHover | AfterFocus,
        AfterAnyAction = AfterClick | AfterGet | AfterSet | AfterFocus,
        AfterClickOrSet = AfterClick | AfterSet,

        BeforeAndAfterClick = BeforeClick | AfterClick
    }
}
