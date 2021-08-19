using System;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the script executor of UI component.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentScriptExecutor<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIComponentScriptExecutor{TOwner}"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        public UIComponentScriptExecutor(IUIComponent<TOwner> component)
        {
            Component = component ?? throw new ArgumentNullException(nameof(component));
            ComponentPartName = "scripts";
        }

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner Execute(string script, params object[] arguments)
        {
            ExecuteScript(script, arguments);

            return Component.Owner;
        }

        /// <summary>
        /// Executes the specified script that returns the result of <typeparamref name="TResult"/> type.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public DataProvider<TResult, TOwner> Execute<TResult>(string script, params object[] arguments)
        {
            return new DataProvider<TResult, TOwner>(
                (UIComponent<TOwner>)Component,
                () => ConvertResult<TResult>(ExecuteScript(script, arguments)),
                "script result");
        }

        /// <summary>
        /// Executes the specified script against the <see cref="UIComponent.Scope"/> element of the current component.
        /// It means that the first argument (<c>arguments[0]</c>) passed into the script is the component's element.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner ExecuteAgainst(string script, params object[] arguments)
        {
            object[] combinedArguments = new object[] { Component.Scope }.Concat(arguments).ToArray();

            return Execute(script, combinedArguments);
        }

        /// <summary>
        /// Executes the specified script against the <see cref="UIComponent.Scope"/> element of the current component.
        /// The script should return the result of <typeparamref name="TResult"/> type.
        /// It means that the first argument (<c>arguments[0]</c>) passed into the script is the component's element.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public DataProvider<TResult, TOwner> ExecuteAgainst<TResult>(string script, params object[] arguments)
        {
            object[] combinedArguments = new object[] { Component.Scope }.Concat(arguments).ToArray();

            return Execute<TResult>(script, combinedArguments);
        }

        /// <summary>
        /// Executes the specified asynchronous script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner ExecuteAsync(string script, params object[] arguments)
        {
            ExecuteAsyncScript(script, arguments);

            return Component.Owner;
        }

        /// <summary>
        /// Executes the specified asynchronous script that returns the result of <typeparamref name="TResult"/> type.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public DataProvider<TResult, TOwner> ExecuteAsync<TResult>(string script, params object[] arguments)
        {
            return new DataProvider<TResult, TOwner>(
                (UIComponent<TOwner>)Component,
                () => ConvertResult<TResult>(ExecuteAsyncScript(script, arguments)),
                "script result");
        }

        /// <summary>
        /// Executes the specified asynchronous script against the <see cref="UIComponent.Scope"/> element of the current component.
        /// It means that the first argument (<c>arguments[0]</c>) passed into the script is the component's element.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner ExecuteAsyncAgainst(string script, params object[] arguments)
        {
            object[] combinedArguments = new object[] { Component.Scope }.Concat(arguments).ToArray();

            return ExecuteAsync(script, combinedArguments);
        }

        /// <summary>
        /// Executes the specified asynchronous script against the <see cref="UIComponent.Scope"/> element of the current component.
        /// The script should return the result of <typeparamref name="TResult"/> type.
        /// It means that the first argument (<c>arguments[0]</c>) passed into the script is the component's element.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>An instance of the owner page object.</returns>
        public DataProvider<TResult, TOwner> ExecuteAsyncAgainst<TResult>(string script, params object[] arguments)
        {
            object[] combinedArguments = new object[] { Component.Scope }.Concat(arguments).ToArray();

            return ExecuteAsync<TResult>(script, combinedArguments);
        }

        private object ExecuteScript(string script, object[] arguments)
        {
            return Component.Owner.Driver.ExecuteScriptWithLogging(script, arguments);
        }

        private object ExecuteAsyncScript(string script, object[] arguments)
        {
            return Component.Owner.Driver.ExecuteAsyncScriptWithLogging(script, arguments);
        }

        /// <summary>
        /// <para>
        /// Sets the value to the <see cref="UIComponent.Scope"/> element of the current component.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// arguments[0].value = arguments[1];
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner SetValue(string value)
        {
            return ExecuteAgainst(
                "arguments[0].value = arguments[1];",
                value ?? string.Empty);
        }

        /// <summary>
        /// <para>
        /// Adds the specified value to the current value of the <see cref="UIComponent.Scope"/> element of the current component.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// var currentValue = arguments[0].value;
        /// arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner AddValue(string value)
        {
            return ExecuteAgainst(
                "var currentValue = arguments[0].value;" +
                "arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];",
                value ?? string.Empty);
        }

        /// <summary>
        /// <para>
        /// Sets the value to the <see cref="UIComponent.Scope"/> element of the current component and dispatches 'change' event.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// arguments[0].value = arguments[1];
        /// arguments[0].dispatchEvent(new Event('change'));
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner SetValueAndDispatchChangeEvent(string value)
        {
            return ExecuteAgainst(
                "arguments[0].value = arguments[1];" +
                "arguments[0].dispatchEvent(new Event('change'));",
                value ?? string.Empty);
        }

        /// <summary>
        /// <para>
        /// Adds the specified value to the current value of the <see cref="UIComponent.Scope"/> element of the current component and dispatches 'change' event.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// var currentValue = arguments[0].value;
        /// arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];
        /// arguments[0].dispatchEvent(new Event('change'));
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner AddValueAndDispatchChangeEvent(string value)
        {
            return ExecuteAgainst(
                "var currentValue = arguments[0].value;" +
                "arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];" +
                "arguments[0].dispatchEvent(new Event('change'));",
                value ?? string.Empty);
        }

        /// <summary>
        /// <para>
        /// Dispatches the specified event.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// arguments[0].dispatchEvent(new Event(arguments[1]));
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner DispatchEvent(string eventName)
        {
            eventName.CheckNotNullOrWhitespace(nameof(eventName));

            return ExecuteAgainst(
                "arguments[0].dispatchEvent(new Event(arguments[1]));",
                eventName);
        }

        /// <summary>
        /// <para>
        /// Clicks the <see cref="UIComponent.Scope"/> element of the current component.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// arguments[0].click();
        /// </code>
        /// </para>
        /// </summary>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner Click()
        {
            return ExecuteAgainst("arguments[0].click();");
        }

        /// <summary>
        /// <para>
        /// Sets focus to the <see cref="UIComponent.Scope"/> element of the current component.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// arguments[0].focus();
        /// </code>
        /// </para>
        /// </summary>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner Focus()
        {
            return ExecuteAgainst("arguments[0].focus();");
        }

        /// <summary>
        /// <para>
        /// Scrolls to the <see cref="UIComponent.Scope"/> element of the current component.
        /// </para>
        /// <para>
        /// Executable script:
        /// <code>
        /// arguments[0].scrollIntoView();
        /// </code>
        /// </para>
        /// </summary>
        /// <returns>An instance of the owner page object.</returns>
        public TOwner ScrollIntoView()
        {
            return ExecuteAgainst("arguments[0].scrollIntoView();");
        }

        private static TResult ConvertResult<TResult>(object result)
        {
            IObjectConverter objectConverter = AtataContext.Current?.ObjectConverter ?? new ObjectConverter();
            return (TResult)objectConverter.Convert(result, typeof(TResult));
        }
    }
}
