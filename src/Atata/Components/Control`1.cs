using System;

namespace Atata
{
    /// <summary>
    /// Represents the base class for the controls.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "control")]
    public class Control<TOwner> : UIComponent<TOwner>, IControl<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the control is enabled.
        /// </summary>
        public DataProvider<bool, TOwner> IsEnabled => GetOrCreateDataProvider("enabled state", GetIsEnabled);

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public new UIComponentVerificationProvider<Control<TOwner>, TOwner> Should => new UIComponentVerificationProvider<Control<TOwner>, TOwner>(this);

        protected virtual bool GetIsEnabled()
        {
            return Scope.Enabled;
        }

        /// <summary>
        /// Clicks the control.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner Click()
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.Start(new ClickLogSection(this));

            OnClick();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnClick()
        {
            Scope.Click();
        }

        /// <summary>
        /// Clicks the control and performs the navigation to the page object of <typeparamref name="TNavigateTo"/> type.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
        /// <param name="navigateToPageObject">The page object instance to navigate to.</param>
        /// <param name="temporarily">
        /// If set to <c>true</c> navigates temporarily preserving current page object state.
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
                ?? Metadata.Get<GoTemporarilyAttribute>(x => x.At(AttributeLevels.DeclaredAndComponent))?.IsTemporarily
                ?? false;

            TNavigateTo pageObject = navigateToPageObject
                ?? (TNavigateTo)Metadata.Get<NavigationPageObjectCreatorAttribute>(x => x.At(AttributeLevels.DeclaredAndComponent))?.Creator?.Invoke();

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
            Log.Start(new HoverLogSection(this));

            OnHover();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterHover);

            return Owner;
        }

        protected virtual void OnHover()
        {
            Driver.Perform(actions => actions.MoveToElement(Scope));
        }

        /// <summary>
        /// Focuses the control.
        /// Also executes <see cref="TriggerEvents.BeforeFocus" /> and <see cref="TriggerEvents.AfterFocus" /> triggers.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner Focus()
        {
            ExecuteTriggers(TriggerEvents.BeforeFocus);
            Log.Start(new FocusLogSection(this));

            OnFocus();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterFocus);

            return Owner;
        }

        protected virtual void OnFocus()
        {
            Driver.ExecuteScript("arguments[0].focus();", Scope);
        }

        /// <summary>
        /// Double-clicks the control.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner DoubleClick()
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.Start(new DoubleClickLogSection(this));

            OnDoubleClick();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnDoubleClick()
        {
            Driver.Perform(actions => actions.DoubleClick(Scope));
        }

        /// <summary>
        /// Double-clicks the control and performs the navigation to the page object of <typeparamref name="TNavigateTo"/> type.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
        /// <param name="navigateToPageObject">The page object instance to navigate to.</param>
        /// <param name="temporarily">
        /// If set to <c>true</c> navigates temporarily preserving current page object state.
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
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner RightClick()
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.Start(new RightClickLogSection(this));

            OnRightClick();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnRightClick()
        {
            Driver.Perform(actions => actions.ContextClick(Scope));
        }

        /// <summary>
        /// Drags and drops the control to the target control returned by <paramref name="targetSelector"/>.
        /// By default uses <see cref="DragAndDropUsingActionsAttribute"/>.
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
        /// By default uses <see cref="DragAndDropUsingActionsAttribute"/>.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <param name="target">The target control.</param>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner DragAndDropTo(Control<TOwner> target)
        {
            target.CheckNotNull(nameof(target));

            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.Start(new DragAndDropToComponentLogSection(this, target));

            OnDragAndDropTo(target);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnDragAndDropTo(Control<TOwner> target)
        {
            var behavior = Metadata.Get<DragAndDropBehaviorAttribute>()
                ?? new DragAndDropUsingActionsAttribute();

            behavior.Execute(this, target);
        }

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
            Log.Start(new DragAndDropToOffsetLogSection(this, offsetX, offsetY));

            OnDragAndDropToOffset(offsetX, offsetY);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnDragAndDropToOffset(int offsetX, int offsetY)
        {
            Driver.Perform(x => x.DragAndDropToOffset(Scope, offsetX, offsetY));
        }

        /// <summary>
        /// Scrolls to the control.
        /// By default uses <see cref="ScrollUsingMoveToElementAttribute"/> behavior.
        /// Also executes <see cref="TriggerEvents.BeforeScroll" /> and <see cref="TriggerEvents.AfterScroll" /> triggers.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner ScrollTo()
        {
            ExecuteTriggers(TriggerEvents.BeforeScroll);
            Log.Start(new ScrollToComponentLogSection(this));

            OnScrollTo();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterScroll);

            return Owner;
        }

        protected virtual void OnScrollTo()
        {
            var behavior = Metadata.Get<ScrollBehaviorAttribute>() ?? new ScrollUsingMoveToElementAttribute();

            behavior.Execute(this);
        }
    }
}
