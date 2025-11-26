namespace Atata;

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
    Init = 1 << 0,

    /// <summary>
    /// Occurs upon the page object deinitialization.
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
    /// Occurs before the value is taken from the control.
    /// </summary>
    BeforeGet = 1 << 6,

    /// <summary>
    /// Occurs after the value is taken from the control.
    /// </summary>
    AfterGet = 1 << 7,

    /// <summary>
    /// Occurs before the value is set to the control.
    /// </summary>
    BeforeSet = 1 << 8,

    /// <summary>
    /// Occurs after the value is set to the control.
    /// </summary>
    AfterSet = 1 << 9,

    /// <summary>
    /// Occurs before the click on the control.
    /// </summary>
    BeforeClick = 1 << 10,

    /// <summary>
    /// Occurs after the click on the control.
    /// </summary>
    AfterClick = 1 << 11,

    /// <summary>
    /// Occurs before the hover on the control.
    /// </summary>
    BeforeHover = 1 << 12,

    /// <summary>
    /// Occurs after the hover on the control.
    /// </summary>
    AfterHover = 1 << 13,

    /// <summary>
    /// Occurs before the control gets the focus.
    /// </summary>
    BeforeFocus = 1 << 14,

    /// <summary>
    /// Occurs after the control gets the focus.
    /// </summary>
    AfterFocus = 1 << 15,

    /// <summary>
    /// Occurs before the control loses the focus.
    /// </summary>
    BeforeBlur = 1 << 16,

    /// <summary>
    /// Occurs after the control loses the focus.
    /// </summary>
    AfterBlur = 1 << 17,

    /// <summary>
    /// Occurs before the scrolling to control.
    /// </summary>
    BeforeScroll = 1 << 18,

    /// <summary>
    /// Occurs after the scrolling to control.
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
    /// The combination of <see cref="BeforeClick"/>, <see cref="BeforeHover"/> and <see cref="BeforeFocus"/>.
    /// </summary>
    BeforeClickOrHoverOrFocus = BeforeClick | BeforeHover | BeforeFocus,

    /// <summary>
    /// The combination of <see cref="BeforeClick"/>, <see cref="BeforeGet"/>, <see cref="BeforeSet"/>, <see cref="BeforeFocus"/>, <see cref="BeforeBlur"/> and <see cref="BeforeScroll"/>.
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
    /// The combination of <see cref="AfterClick"/>, <see cref="AfterHover"/> and <see cref="AfterFocus"/>.
    /// </summary>
    AfterClickOrHoverOrFocus = AfterClick | AfterHover | AfterFocus,

    /// <summary>
    /// The combination of <see cref="AfterClick"/>, <see cref="AfterGet"/>, <see cref="AfterSet"/>, <see cref="AfterFocus"/>, <see cref="AfterBlur"/> and <see cref="AfterScroll"/>.
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
