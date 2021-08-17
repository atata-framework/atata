using System;

namespace Atata
{
    /// <summary>
    /// Represents the base class for the controls.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "control")]
    [ClicksUsingClickMethod]
    [DoubleClicksUsingActions]
    [RightClicksUsingActions]
    [FocusesUsingScript]
    [DragsAndDropsUsingActions]
    [ScrollsUsingActions]
    public class Control<TOwner> : UIComponent<TOwner>, IControl<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the source of the scope.
        /// The default value is <see cref="ScopeSource.Parent"/>.
        /// </summary>
        public sealed override ScopeSource ScopeSource =>
            Metadata?.ResolveFindAttribute()?.ScopeSource ?? ScopeSource.Parent;

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value
        /// indicating whether the control is enabled.
        /// </summary>
        public DataProvider<bool, TOwner> IsEnabled =>
            GetOrCreateDataProvider("enabled state", GetIsEnabled);

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public new UIComponentVerificationProvider<Control<TOwner>, TOwner> Should =>
            new UIComponentVerificationProvider<Control<TOwner>, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public new UIComponentVerificationProvider<Control<TOwner>, TOwner> ExpectTo =>
            Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public new UIComponentVerificationProvider<Control<TOwner>, TOwner> WaitTo =>
            Should.Using<WaitingVerificationStrategy>();

        protected virtual bool GetIsEnabled() =>
            Scope.Enabled;

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
        public TNavigateTo ClickAndGo<TNavigateTo>(TNavigateTo navigateToPageObject = null, bool? temporarily = null)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            Click();

            return OnGo(navigateToPageObject, temporarily);
        }

        protected virtual TNavigateTo OnGo<TNavigateTo>(TNavigateTo navigateToPageObject = null, bool? temporarily = null)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            bool isTemporarily = temporarily
                ?? Metadata.Get<GoTemporarilyAttribute>()?.IsTemporarily
                ?? false;

            TNavigateTo pageObject = navigateToPageObject
                ?? (TNavigateTo)Metadata.Get<NavigationPageObjectCreatorAttribute>()?.Creator?.Invoke();

            return Go.To(pageObject: pageObject, navigate: false, temporarily: isTemporarily);
        }

        /// <summary>
        /// Hovers the control.
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

        protected virtual void OnHover()
        {
            Driver.Perform(actions => actions.MoveToElement(Scope));
        }

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
        public TNavigateTo DoubleClickAndGo<TNavigateTo>(TNavigateTo navigateToPageObject = null, bool? temporarily = null)
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
            targetSelector.CheckNotNull(nameof(targetSelector));

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
            target.CheckNotNull(nameof(target));

            ExecuteTriggers(TriggerEvents.BeforeClick);

            Log.ExecuteSection(
                new DragAndDropToComponentLogSection(this, target),
                () => OnDragAndDropTo(target));

            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        /// <summary>
        /// Drag and drops the control to the target control by executing <see cref="DragAndDropBehaviorAttribute"/>.
        /// </summary>
        /// <param name="target">The target.</param>
        protected virtual void OnDragAndDropTo(Control<TOwner> target) =>
            ExecuteBehavior<DragAndDropBehaviorAttribute>(x => x.Execute(this, target));

        /// <summary>
        /// Drags and drops the control to the specified offset.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
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

        protected virtual void OnDragAndDropToOffset(int offsetX, int offsetY)
        {
            Driver.Perform(x => x.DragAndDropToOffset(Scope, offsetX, offsetY));
        }

        /// <summary>
        /// Scrolls to the control.
        /// Executes an associated with the component <see cref="ScrollBehaviorAttribute"/>
        /// that is <see cref="ScrollsUsingActionsAttribute"/> by default.
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
}
