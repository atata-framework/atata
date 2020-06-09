using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the text area control (<c>&lt;textarea&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("textarea", IgnoreNameEndings = "TextArea", ComponentTypeName = "text area")]
    [FindByLabel]
    public class TextArea<TOwner> : EditableField<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string GetValue()
        {
            return Attributes.Value;
        }

        protected override void SetValue(string value)
        {
            Scope.FillInWith(value);
        }

        /// <summary>
        /// Appends the specified value.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The owner page object.</returns>
        public TOwner Append(string value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataAdditionLogSection(this, value) { ActionText = "Append" });

            Scope.SendKeys(Keys.End + value);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        /// <summary>
        /// Clears the value.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <returns>The owner page object.</returns>
        public TOwner Clear()
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataClearingLogSection(this));

            OnClear();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        protected virtual void OnClear()
        {
            Scope.Clear();
        }
    }
}
