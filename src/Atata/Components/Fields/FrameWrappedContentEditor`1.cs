using System;

namespace Atata
{
    /// <summary>
    /// Represents the frame-wrapped content editor control.
    /// This control is good to use for iframe-based WYSIWYG editors.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    [ControlDefinition("iframe", ComponentTypeName = "frame-wrapped content editor")]
    public class FrameWrappedContentEditor<TOwner> : EditableTextField<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the content editor.
        /// Finds the first one inside an <c>iframe</c> element.
        /// </summary>
        [Name("Inner")]
        [FindSettings(ScopeSource = ScopeSource.Page)]
        [TraceLog]
        protected ContentEditor<TOwner> ContentEditor { get; private set; }

        /// <summary>
        /// Gets the frame control.
        /// By default returns <c>this</c>.
        /// </summary>
        /// <returns>The control representing <c>iframe</c> element.</returns>
        protected virtual Control<TOwner> GetFrameControl() => this;

        protected override string GetValue()
        {
            string value = null;

            DoWithinFrame(() =>
            {
                value = ContentEditor.Value;
            });

            return value;
        }

        protected override void SetValue(string value)
        {
            DoWithinFrame(() =>
            {
                ContentEditor.Set(value);
            });
        }

        protected override void OnClear()
        {
            DoWithinFrame(() =>
            {
                ContentEditor.Clear();
            });
        }

        protected override void OnType(string value)
        {
            DoWithinFrame(() =>
            {
                ContentEditor.Type(value);
            });
        }

        /// <summary>
        /// Performs the action within the frame provided by <see cref="GetFrameControl()"/> method.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void DoWithinFrame(Action action)
        {
            var frameControl = GetFrameControl();

            Owner.SwitchToFrame(frameControl);

            try
            {
                action?.Invoke();
            }
            finally
            {
                Owner.SwitchToRoot();
            }
        }
    }
}
