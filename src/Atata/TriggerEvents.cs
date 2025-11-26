namespace Atata;

/// <summary>
/// Specifies the trigger events.
/// </summary>
[Flags]
public enum TriggerEvents
{
    /// <summary>
    /// No events.
    /// </summary>
    None = 0,

    /// <summary>
    /// Occurs during page object initialization.
    /// </summary>
    Init = 1 << 0,

    /// <summary>
    /// Occurs during page object deinitialization.
    /// </summary>
    DeInit = 1 << 1,

    /// <summary>
    /// Occurs when a page object transition in is completed.
    /// That is, navigation to the current page object occurred in the same browser tab
    /// by interacting with the previous page object, rather than by directly navigating to a URL.
    /// </summary>
    PageObjectTransitionIn = 1 << 2,

    /// <summary>
    /// Occurs when a page object transition out is completed.
    /// That is, navigation to the next page object occurred in the same browser tab
    /// by interacting with the current page object, rather than by directly navigating to a URL.
    /// </summary>
    PageObjectTransitionOut = 1 << 3,

    /// <summary>
    /// Occurs before any access to the component.
    /// </summary>
    BeforeAccess = 1 << 4,

    /// <summary>
    /// Occurs after any access to the component.
    /// </summary>
    AfterAccess = 1 << 5,

    /// <summary>
    /// Occurs before a value is retrieved from the control.
    /// </summary>
    BeforeGet = 1 << 6,

    /// <summary>
    /// Occurs after a value is retrieved from the control.
    /// </summary>
    AfterGet = 1 << 7,

    /// <summary>
    /// Occurs before a value is set to the control.
    /// </summary>
    BeforeSet = 1 << 8,

    /// <summary>
    /// Occurs after a value is set to the control.
    /// </summary>
    AfterSet = 1 << 9,

    /// <summary>
    /// Occurs before clicking the control.
    /// </summary>
    BeforeClick = 1 << 10,

    /// <summary>
    /// Occurs after clicking the control.
    /// </summary>
    AfterClick = 1 << 11,

    /// <summary>
    /// Occurs before hovering over the control.
    /// </summary>
    BeforeHover = 1 << 12,

    /// <summary>
    /// Occurs after hovering over the control.
    /// </summary>
    AfterHover = 1 << 13,

    /// <summary>
    /// Occurs before the control receives focus.
    /// </summary>
    BeforeFocus = 1 << 14,

    /// <summary>
    /// Occurs after the control receives focus.
    /// </summary>
    AfterFocus = 1 << 15,

    /// <summary>
    /// Occurs before the control loses focus.
    /// </summary>
    BeforeBlur = 1 << 16,

    /// <summary>
    /// Occurs after the control loses focus.
    /// </summary>
    AfterBlur = 1 << 17,

    /// <summary>
    /// Occurs before scrolling to the control.
    /// </summary>
    BeforeScroll = 1 << 18,

    /// <summary>
    /// Occurs after scrolling to the control.
    /// </summary>
    AfterScroll = 1 << 19,

    /// <summary>
    /// The combination of <see cref="BeforeGet"/> and <see cref="BeforeSet"/>.
    /// </summary>
    BeforeGetOrSet = BeforeGet | BeforeSet,

    /// <summary>
    /// The combination of <see cref="BeforeClick"/> and <see cref="BeforeHover"/>.
    /// </summary>
    BeforeClickOrHover = BeforeClick | BeforeHover,

    /// <summary>
    /// The combination of <see cref="BeforeClick"/> and <see cref="BeforeFocus"/>.
    /// </summary>
    BeforeClickOrFocus = BeforeClick | BeforeFocus,

    /// <summary>
    /// The combination of <see cref="BeforeClick"/>, <see cref="BeforeHover"/>, and <see cref="BeforeFocus"/>.
    /// </summary>
    BeforeClickOrHoverOrFocus = BeforeClick | BeforeHover | BeforeFocus,

    /// <summary>
    /// The combination of <see cref="BeforeClick"/>, <see cref="BeforeGet"/>, <see cref="BeforeSet"/>, <see cref="BeforeFocus"/>, <see cref="BeforeBlur"/>, and <see cref="BeforeScroll"/>.
    /// </summary>
    BeforeAnyAction = BeforeClick | BeforeGet | BeforeSet | BeforeFocus | BeforeBlur | BeforeScroll,

    /// <summary>
    /// The combination of <see cref="AfterGet"/> and <see cref="AfterSet"/>.
    /// </summary>
    AfterGetOrSet = AfterGet | AfterSet,

    /// <summary>
    /// The combination of <see cref="AfterClick"/> and <see cref="AfterHover"/>.
    /// </summary>
    AfterClickOrHover = AfterClick | AfterHover,

    /// <summary>
    /// The combination of <see cref="AfterClick"/> and <see cref="AfterFocus"/>.
    /// </summary>
    AfterClickOrFocus = AfterClick | AfterFocus,

    /// <summary>
    /// The combination of <see cref="AfterClick"/>, <see cref="AfterHover"/>, and <see cref="AfterFocus"/>.
    /// </summary>
    AfterClickOrHoverOrFocus = AfterClick | AfterHover | AfterFocus,

    /// <summary>
    /// The combination of <see cref="AfterClick"/>, <see cref="AfterGet"/>, <see cref="AfterSet"/>, <see cref="AfterFocus"/>, <see cref="AfterBlur"/>, and <see cref="AfterScroll"/>.
    /// </summary>
    AfterAnyAction = AfterClick | AfterGet | AfterSet | AfterFocus | AfterBlur | AfterScroll,

    /// <summary>
    /// The combination of <see cref="AfterClick"/> and <see cref="AfterSet"/>.
    /// </summary>
    AfterClickOrSet = AfterClick | AfterSet,

    /// <summary>
    /// The combination of <see cref="BeforeClick"/> and <see cref="AfterClick"/>.
    /// </summary>
    BeforeAndAfterClick = BeforeClick | AfterClick
}
