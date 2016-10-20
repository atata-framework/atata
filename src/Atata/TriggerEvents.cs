using System;

namespace Atata
{
    /// <summary>
    /// Specifies the trigger events.
    /// </summary>
    [Flags]
    public enum TriggerEvents
    {
        /// <summary>
        /// None of the events.
        /// </summary>
        None = 0,

        /// <summary>
        /// Occurs upon the page object initialization.
        /// </summary>
        OnPageObjectInit = 1 << 0,

        /// <summary>
        /// Occurs upon the page object de-initialization.
        /// </summary>
        OnPageObjectLeave = 1 << 1,

        /// <summary>
        /// Occurs before the value is taken from the control.
        /// </summary>
        BeforeGet = 1 << 2,

        /// <summary>
        /// Occurs after the value is taken from the control.
        /// </summary>
        AfterGet = 1 << 3,

        /// <summary>
        /// Occurs before the value is set to the control.
        /// </summary>
        BeforeSet = 1 << 4,

        /// <summary>
        /// Occurs after the value is set to the control.
        /// </summary>
        AfterSet = 1 << 5,

        /// <summary>
        /// Occurs before the click on the control.
        /// </summary>
        BeforeClick = 1 << 6,

        /// <summary>
        /// Occurs after the click on the control.
        /// </summary>
        AfterClick = 1 << 7,

        /// <summary>
        /// Occurs before the hover on the control.
        /// </summary>
        BeforeHover = 1 << 8,

        /// <summary>
        /// Occurs after the hover on the control.
        /// </summary>
        AfterHover = 1 << 9,

        /// <summary>
        /// Occurs before the control gets the focus.
        /// </summary>
        BeforeFocus = 1 << 10,

        /// <summary>
        /// Occurs after the control gets the focus.
        /// </summary>
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
