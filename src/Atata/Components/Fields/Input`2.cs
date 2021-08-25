using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the input control (<c>&lt;input&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[not(@type) or (@type!='button' and @type!='submit' and @type!='reset')]", ComponentTypeName = "input")]
    [FindByLabel]
    public class Input<T, TOwner> : EditableTextField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Appends the specified value.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The owner page object.</returns>
        [Obsolete("Use Type(...) instead.")] // Obsolete since v1.9.0.
        public TOwner Append(string value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);

            Log.ExecuteSection(
                new ValueChangeLogSection(this, "Append", value),
                () => Scope.SendKeysWithLogging(Keys.End + value));

            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }
    }
}
