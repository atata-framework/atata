﻿namespace Atata;

/// <summary>
/// Represents the base class for the controls.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition(ComponentTypeName = "control")]
[ClicksUsingClickMethod]
[DoubleClicksUsingActions]
[RightClicksUsingActions]
[HoversUsingActions]
[FocusesUsingScript]
[BlursUsingScript]
[DragsAndDropsUsingActions]
[DragsAndDropsToOffsetUsingActions]
[ScrollsUsingScrollToElementAction]
public class Control<TOwner> : UIComponent<TOwner>, IControl<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <inheritdoc/>
    public sealed override WebDriverSession Session => Owner.Session;

    /// <summary>
    /// Gets the source of the scope.
    /// The default value is <see cref="ScopeSource.Parent"/>.
    /// </summary>
    public sealed override ScopeSource ScopeSource =>
        Metadata?.ResolveFindAttribute()?.ResolveScopeSource(Metadata) ?? ScopeSource.Parent;

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> for the value
    /// indicating whether the control is enabled.
    /// </summary>
    public ValueProvider<bool, TOwner> IsEnabled =>
        CreateValueProvider("enabled state", GetIsEnabled);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> for the value
    /// indicating whether the control is focused.
    /// </summary>
    public ValueProvider<bool, TOwner> IsFocused =>
        CreateValueProvider("focused state", GetIsFocused);

    /// <inheritdoc cref="UIComponent{TOwner}.Should"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new UIComponentVerificationProvider<Control<TOwner>, TOwner> Should =>
        new(this);

    /// <inheritdoc cref="UIComponent{TOwner}.ExpectTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new UIComponentVerificationProvider<Control<TOwner>, TOwner> ExpectTo =>
        Should.Using(ExpectationVerificationStrategy.Instance);

    /// <inheritdoc cref="UIComponent{TOwner}.WaitTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new UIComponentVerificationProvider<Control<TOwner>, TOwner> WaitTo =>
        Should.Using(WaitingVerificationStrategy.Instance);

    protected virtual bool GetIsEnabled() =>
        Scope.Enabled;

    protected virtual bool GetIsFocused() =>
        Script.IsFocused();

    /// <summary>
    /// Clicks the control.
    /// Executes an associated with the component <see cref="ClickBehaviorAttribute"/>
    /// that is <see cref="ClicksUsingClickMethodAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner Click()
    {
        ExecuteTriggers(TriggerEvents.BeforeClick);

        Log.ExecuteSection(
            new ClickLogSection(this),
            OnClick);

        ExecuteTriggers(TriggerEvents.AfterClick);

        return Owner;
    }

    /// <summary>
    /// Clicks the control by executing <see cref="ClickBehaviorAttribute"/>.
    /// </summary>
    protected virtual void OnClick() =>
        ExecuteBehavior<ClickBehaviorAttribute>(x => x.Execute(this));

    /// <summary>
    /// Clicks the control and performs the navigation to the page object of <typeparamref name="TNavigateTo"/> type.
    /// Executes an associated with the component <see cref="ClickBehaviorAttribute"/>
    /// that is <see cref="ClicksUsingClickMethodAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <param name="navigateToPageObject">The page object instance to navigate to.</param>
    /// <param name="temporarily">
    /// If set to <see langword="true"/> navigates temporarily preserving current page object state.
    /// If is not set, checks <see cref="GoTemporarilyAttribute"/>.</param>
    /// <returns>The instance of <typeparamref name="TNavigateTo"/>.</returns>
    public TNavigateTo ClickAndGo<TNavigateTo>(TNavigateTo? navigateToPageObject = null, bool? temporarily = null)
        where TNavigateTo : PageObject<TNavigateTo>
    {
        Click();

        return OnGo(navigateToPageObject, temporarily);
    }

    protected virtual TNavigateTo OnGo<TNavigateTo>(TNavigateTo? navigateToPageObject = null, bool? temporarily = null)
        where TNavigateTo : PageObject<TNavigateTo>
    {
        bool isTemporarily = temporarily
            ?? Metadata.Get<GoTemporarilyAttribute>()?.IsTemporarily
            ?? false;

        TNavigateTo? pageObject = navigateToPageObject
            ?? Metadata.Get<NavigationPageObjectCreatorAttribute>()?.Creator?.Invoke() as TNavigateTo;

        return Go.To(pageObject: pageObject, navigate: false, temporarily: isTemporarily);
    }

    /// <summary>
    /// Hovers the control.
    /// Executes an associated with the component <see cref="HoverBehaviorAttribute"/>
    /// that is <see cref="HoversUsingActionsAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeHover" /> and <see cref="TriggerEvents.AfterHover" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner Hover()
    {
        ExecuteTriggers(TriggerEvents.BeforeHover);

        Log.ExecuteSection(
            new HoverLogSection(this),
            OnHover);

        ExecuteTriggers(TriggerEvents.AfterHover);

        return Owner;
    }

    /// <summary>
    /// Hovers the control by executing <see cref="HoverBehaviorAttribute"/>.
    /// </summary>
    protected virtual void OnHover() =>
        ExecuteBehavior<HoverBehaviorAttribute>(x => x.Execute(this));

    /// <summary>
    /// Focuses the control.
    /// Executes an associated with the component <see cref="FocusBehaviorAttribute"/>
    /// that is <see cref="FocusesUsingScriptAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeFocus" /> and <see cref="TriggerEvents.AfterFocus" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner Focus()
    {
        ExecuteTriggers(TriggerEvents.BeforeFocus);

        Log.ExecuteSection(
            new FocusLogSection(this),
            OnFocus);

        ExecuteTriggers(TriggerEvents.AfterFocus);

        return Owner;
    }

    /// <summary>
    /// Focuses the control by executing <see cref="FocusBehaviorAttribute"/>.
    /// </summary>
    protected virtual void OnFocus() =>
        ExecuteBehavior<FocusBehaviorAttribute>(x => x.Execute(this));

    /// <summary>
    /// Removes focus from the control.
    /// Executes an associated with the component <see cref="BlurBehaviorAttribute"/>
    /// that is <see cref="BlursUsingScriptAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeBlur" /> and <see cref="TriggerEvents.AfterBlur" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner Blur()
    {
        ExecuteTriggers(TriggerEvents.BeforeBlur);

        Log.ExecuteSection(
            new BlurLogSection(this),
            OnBlur);

        ExecuteTriggers(TriggerEvents.AfterBlur);

        return Owner;
    }

    /// <summary>
    /// Removes focus from the control by executing <see cref="BlurBehaviorAttribute"/>.
    /// </summary>
    protected virtual void OnBlur() =>
        ExecuteBehavior<BlurBehaviorAttribute>(x => x.Execute(this));

    /// <summary>
    /// Double-clicks the control.
    /// Executes an associated with the component <see cref="DoubleClickBehaviorAttribute"/>
    /// that is <see cref="DoubleClicksUsingActionsAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner DoubleClick()
    {
        ExecuteTriggers(TriggerEvents.BeforeClick);

        Log.ExecuteSection(
            new DoubleClickLogSection(this),
            OnDoubleClick);

        ExecuteTriggers(TriggerEvents.AfterClick);

        return Owner;
    }

    /// <summary>
    /// Double-clicks the control by executing <see cref="DoubleClickBehaviorAttribute"/>.
    /// </summary>
    protected virtual void OnDoubleClick() =>
        ExecuteBehavior<DoubleClickBehaviorAttribute>(x => x.Execute(this));

    /// <summary>
    /// Double-clicks the control and performs the navigation to the page object of <typeparamref name="TNavigateTo"/> type.
    /// Executes an associated with the component <see cref="DoubleClickBehaviorAttribute"/>
    /// that is <see cref="DoubleClicksUsingActionsAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <param name="navigateToPageObject">The page object instance to navigate to.</param>
    /// <param name="temporarily">
    /// If set to <see langword="true"/> navigates temporarily preserving current page object state.
    /// If is not set, checks <see cref="GoTemporarilyAttribute"/>.</param>
    /// <returns>The instance of <typeparamref name="TNavigateTo"/>.</returns>
    public TNavigateTo DoubleClickAndGo<TNavigateTo>(TNavigateTo? navigateToPageObject = null, bool? temporarily = null)
        where TNavigateTo : PageObject<TNavigateTo>
    {
        DoubleClick();

        return OnGo(navigateToPageObject, temporarily);
    }

    /// <summary>
    /// Right-clicks the control.
    /// Executes an associated with the component <see cref="RightClickBehaviorAttribute"/>
    /// that is <see cref="RightClicksUsingActionsAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner RightClick()
    {
        ExecuteTriggers(TriggerEvents.BeforeClick);

        Log.ExecuteSection(
            new RightClickLogSection(this),
            OnRightClick);

        ExecuteTriggers(TriggerEvents.AfterClick);

        return Owner;
    }

    /// <summary>
    /// Right-clicks the control by executing <see cref="RightClickBehaviorAttribute"/>.
    /// </summary>
    protected virtual void OnRightClick() =>
        ExecuteBehavior<RightClickBehaviorAttribute>(x => x.Execute(this));

    /// <summary>
    /// Drags and drops the control to the target control returned by <paramref name="targetSelector"/>.
    /// Executes an associated with the component <see cref="DragAndDropBehaviorAttribute"/>
    /// that is <see cref="DragsAndDropsUsingActionsAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <param name="targetSelector">The target control selector.</param>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner DragAndDropTo(Func<TOwner, Control<TOwner>> targetSelector)
    {
        Guard.ThrowIfNull(targetSelector);

        var target = targetSelector(Owner);

        return DragAndDropTo(target);
    }

    /// <summary>
    /// Drags and drops the control to the target control.
    /// Executes an associated with the component <see cref="DragAndDropBehaviorAttribute"/>
    /// that is <see cref="DragsAndDropsUsingActionsAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <param name="target">The target control.</param>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner DragAndDropTo(Control<TOwner> target)
    {
        Guard.ThrowIfNull(target);

        ExecuteTriggers(TriggerEvents.BeforeClick);

        Log.ExecuteSection(
            new DragAndDropToComponentLogSection(this, target),
            () => OnDragAndDropTo(target));

        ExecuteTriggers(TriggerEvents.AfterClick);

        return Owner;
    }

    /// <summary>
    /// Drags and drops the control to the target control by executing <see cref="DragAndDropBehaviorAttribute"/>.
    /// </summary>
    /// <param name="target">The target.</param>
    protected virtual void OnDragAndDropTo(Control<TOwner> target) =>
        ExecuteBehavior<DragAndDropBehaviorAttribute>(x => x.Execute(this, target));

    /// <summary>
    /// Drags and drops the control to the specified offset.
    /// Executes an associated with the component <see cref="DragAndDropToOffsetBehaviorAttribute"/>
    /// that is <see cref="DragsAndDropsToOffsetUsingActionsAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeClick"/> and <see cref="TriggerEvents.AfterClick"/> triggers.
    /// </summary>
    /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
    /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner DragAndDropToOffset(int offsetX, int offsetY)
    {
        ExecuteTriggers(TriggerEvents.BeforeClick);

        Log.ExecuteSection(
            new DragAndDropToOffsetLogSection(this, offsetX, offsetY),
            () => OnDragAndDropToOffset(offsetX, offsetY));

        ExecuteTriggers(TriggerEvents.AfterClick);

        return Owner;
    }

    /// <summary>
    /// Drags and drops the control to the specified offset by executing <see cref="DragAndDropToOffsetBehaviorAttribute"/>.
    /// </summary>
    /// <param name="offsetX">The X offset.</param>
    /// <param name="offsetY">The Y offset.</param>
    protected virtual void OnDragAndDropToOffset(int offsetX, int offsetY) =>
        ExecuteBehavior<DragAndDropToOffsetBehaviorAttribute>(x => x.Execute(this, new Point(offsetX, offsetY)));

    /// <summary>
    /// Scrolls to the control.
    /// Executes an associated with the component <see cref="ScrollBehaviorAttribute"/>
    /// that is <see cref="ScrollsUsingScrollToElementActionAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeScroll" /> and <see cref="TriggerEvents.AfterScroll" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner ScrollTo()
    {
        ExecuteTriggers(TriggerEvents.BeforeScroll);

        Log.ExecuteSection(
            new ScrollToComponentLogSection(this),
            OnScrollTo);

        ExecuteTriggers(TriggerEvents.AfterScroll);

        return Owner;
    }

    /// <summary>
    /// Scrolls to the control by executing <see cref="ScrollBehaviorAttribute"/>.
    /// </summary>
    protected virtual void OnScrollTo() =>
        ExecuteBehavior<ScrollBehaviorAttribute>(x => x.Execute(this));
}
