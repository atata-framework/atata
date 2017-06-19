using System;

namespace Atata
{
    /// <summary>
    /// Represents the base class for the controls.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*", ComponentTypeName = "control")]
    public class Control<TOwner> : UIComponent<TOwner>, IControl<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the control is enabled.
        /// </summary>
        public DataProvider<bool, TOwner> IsEnabled => GetOrCreateDataProvider("enabled", GetIsEnabled);

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public new UIComponentVerificationProvider<Control<TOwner>, TOwner> Should => new UIComponentVerificationProvider<Control<TOwner>, TOwner>(this);

        protected virtual bool GetIsEnabled()
        {
            return Scope.Enabled;
        }

        /// <summary>
        /// Clicks the control. Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
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
        /// Hovers the control. Also executes <see cref="TriggerEvents.BeforeHover" /> and <see cref="TriggerEvents.AfterHover" /> triggers.
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
        /// Focuses the control. Also executes <see cref="TriggerEvents.BeforeFocus" /> and <see cref="TriggerEvents.AfterFocus" /> triggers.
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
        /// Double-clicks the control. Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
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
        /// Right-clicks the control. Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
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
        /// Drags and drops the control to the target control returned by <paramref name="targetControlGetter"/>.
        /// </summary>
        /// <param name="targetControlGetter">The target control getter.</param>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner DragAndDropTo(Func<TOwner, UIComponent> targetControlGetter)
        {
            targetControlGetter.CheckNotNull(nameof(targetControlGetter));

            UIComponent targetControl = targetControlGetter(Owner);

            return DragAndDropTo(targetControl);
        }

        /// <summary>
        /// Drags and drops the control to the target control.
        /// </summary>
        /// <param name="targetControl">The target control.</param>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner DragAndDropTo(UIComponent targetControl)
        {
            targetControl.CheckNotNull(nameof(targetControl));

            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.Start(new DragAndDropToComponentLogSection(this, targetControl));

            OnDragAndDropTo(targetControl);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnDragAndDropTo(UIComponent targetControl)
        {
            Driver.Perform(x => x.DragAndDrop(Scope, targetControl.Scope));
        }

        /// <summary>
        /// Drags and drops the control to the specified offset.
        /// </summary>
        /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner DragAndDropTo(int offsetX, int offsetY)
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.Start(new DragAndDropToOffsetLogSection(this, offsetX, offsetY));

            OnDragAndDropTo(offsetX, offsetY);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnDragAndDropTo(int offsetX, int offsetY)
        {
            Driver.Perform(x => x.DragAndDropToOffset(Scope, offsetX, offsetY));
        }
    }
}
