namespace Atata;

/// <summary>
/// Represents the script executor of UI component.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public class UIComponentScriptExecutor<TOwner> : UIComponentPart<TOwner>
    where TOwner : PageObject<TOwner>
{
    internal UIComponentScriptExecutor(IUIComponent<TOwner> component)
    {
        Component = component;
        ComponentPartName = "scripts";
    }

    private static object[] UnwrapScriptArguments(object[] arguments) =>
        arguments?.Select(arg => arg is UIComponent component ? component.Scope : arg).ToArray();

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
    /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> of the result.</returns>
    public ValueProvider<TResult, TOwner> Execute<TResult>(string script, params object[] arguments) =>
        Component.CreateValueProvider(
            "script result",
            () => ConvertResult<TResult>(ExecuteScript(script, arguments)));

    /// <summary>
    /// Executes the specified script against the <see cref="UIComponent.Scope"/> element of the current component.
    /// It means that the first argument (<c>arguments[0]</c>) passed into the script is the component's element.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="arguments">The arguments.</param>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner ExecuteAgainst(string script, params object[] arguments)
    {
        object[] combinedArguments = [Component, .. arguments];

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
    /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> of the result.</returns>
    public ValueProvider<TResult, TOwner> ExecuteAgainst<TResult>(string script, params object[] arguments)
    {
        object[] combinedArguments = [Component, .. arguments];

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
    /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> of the result.</returns>
    public ValueProvider<TResult, TOwner> ExecuteAsync<TResult>(string script, params object[] arguments) =>
        Component.CreateValueProvider(
            "script result",
            () => ConvertResult<TResult>(ExecuteAsyncScript(script, arguments)));

    /// <summary>
    /// Executes the specified asynchronous script against the <see cref="UIComponent.Scope"/> element of the current component.
    /// It means that the first argument (<c>arguments[0]</c>) passed into the script is the component's element.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="arguments">The arguments.</param>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner ExecuteAsyncAgainst(string script, params object[] arguments)
    {
        object[] combinedArguments = [Component, .. arguments];

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
    /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> of the result.</returns>
    public ValueProvider<TResult, TOwner> ExecuteAsyncAgainst<TResult>(string script, params object[] arguments)
    {
        object[] combinedArguments = [Component, .. arguments];

        return ExecuteAsync<TResult>(script, combinedArguments);
    }

    private object ExecuteScript(string script, object[] arguments)
    {
        object[] unwrappedArguments = UnwrapScriptArguments(arguments);
        return Component.Owner.Driver.AsScriptExecutor().ExecuteScriptWithLogging(script, unwrappedArguments);
    }

    private object ExecuteAsyncScript(string script, object[] arguments)
    {
        object[] unwrappedArguments = UnwrapScriptArguments(arguments);
        return Component.Owner.Driver.AsScriptExecutor().ExecuteAsyncScriptWithLogging(script, unwrappedArguments);
    }

    /// <summary>
    /// <para>
    /// Sets the value to the <see cref="UIComponent.Scope"/> element of the current component.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// arguments[0].value = arguments[1];
    /// </code>
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner SetValue(string value) =>
        ExecuteAgainst(
            "arguments[0].value = arguments[1];",
            value ?? string.Empty);

    /// <summary>
    /// <para>
    /// Adds the specified value to the current value of the <see cref="UIComponent.Scope"/> element of the current component.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// var currentValue = arguments[0].value;
    /// arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];
    /// </code>
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner AddValue(string value) =>
        ExecuteAgainst(
            "var currentValue = arguments[0].value;" +
            "arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];",
            value ?? string.Empty);

    /// <summary>
    /// <para>
    /// Sets the value to the <see cref="UIComponent.Scope"/> element of the current component and dispatches 'change' event.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// arguments[0].value = arguments[1];
    /// arguments[0].dispatchEvent(new Event('change'));
    /// </code>
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner SetValueAndDispatchChangeEvent(string value) =>
        ExecuteAgainst(
            "arguments[0].value = arguments[1];" +
            "arguments[0].dispatchEvent(new Event('change'));",
            value ?? string.Empty);

    /// <summary>
    /// <para>
    /// Adds the specified value to the current value of the <see cref="UIComponent.Scope"/> element of the current component and dispatches 'change' event.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// var currentValue = arguments[0].value;
    /// arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];
    /// arguments[0].dispatchEvent(new Event('change'));
    /// </code>
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner AddValueAndDispatchChangeEvent(string value) =>
        ExecuteAgainst(
            "var currentValue = arguments[0].value;" +
            "arguments[0].value = currentValue ? currentValue + arguments[1] : arguments[1];" +
            "arguments[0].dispatchEvent(new Event('change'));",
            value ?? string.Empty);

    /// <summary>
    /// <para>
    /// Dispatches the specified event.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// arguments[0].dispatchEvent(new Event(arguments[1]));
    /// </code>
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
    /// </para>
    /// <code>
    /// arguments[0].click();
    /// </code>
    /// </summary>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner Click() =>
        ExecuteAgainst("arguments[0].click();");

    /// <summary>
    /// <para>
    /// Sets focus to the <see cref="UIComponent.Scope"/> element of the current component.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// arguments[0].focus();
    /// </code>
    /// </summary>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner Focus() =>
        ExecuteAgainst("arguments[0].focus();");

    /// <summary>
    /// <para>
    /// Gets a value indicating whether the <see cref="UIComponent.Scope"/> element of the current component is focused.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// return arguments[0] === document.activeElement;
    /// </code>
    /// </summary>
    /// <returns>An instance of the <see cref="ValueProvider{TValue, TOwner}"/>.</returns>
    public ValueProvider<bool, TOwner> IsFocused() =>
        ExecuteAgainst<bool>("return arguments[0] === document.activeElement;");

    /// <summary>
    /// <para>
    /// Removes focus from the <see cref="UIComponent.Scope"/> element of the current component.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// arguments[0].blur();
    /// </code>
    /// </summary>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner Blur() =>
        ExecuteAgainst("arguments[0].blur();");

    /// <summary>
    /// <para>
    /// Scrolls to the <see cref="UIComponent.Scope"/> element of the current component.
    /// </para>
    /// <para>
    /// Executable script:
    /// </para>
    /// <code>
    /// arguments[0].scrollIntoView();
    /// </code>
    /// </summary>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner ScrollIntoView() =>
        ExecuteAgainst("arguments[0].scrollIntoView();");

    /// <summary>
    /// Waits until Angular (v2+) has finished rendering and has no outstanding HTTP calls.
    /// The specific Angular app is determined by the value of <see cref="AngularSettings.RootSelector"/>,
    /// which is <c>"[ng-app]"</c> by default.
    /// </summary>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner WaitForAngular() =>
        WaitForAngular(AngularSettings.RootSelector);

    /// <summary>
    /// Waits until Angular (v2+) has finished rendering and has no outstanding HTTP calls.
    /// The specific Angular app is determined by the <paramref name="rootSelector"/>.
    /// </summary>
    /// <param name="rootSelector">The root selector.</param>
    /// <returns>An instance of the owner page object.</returns>
    public TOwner WaitForAngular(string rootSelector) =>
        ExecuteAsync(
            """
            var rootSelector = arguments[0];
            var callback = arguments[1];

            if (window.getAngularTestability) {
              if (rootSelector) {
                var testability = null;
                var el = document.querySelector(rootSelector);
                try {
                  testability = window.getAngularTestability(el);
                }
                catch (e) { }
                if (testability) {
                  testability.whenStable(callback);
                  return;
                }
              }

              var testabilities = window.getAllAngularTestabilities();
              var count = testabilities.length;

              if (count === 0) {
                callback();
                return;
              }

              var decrement = function () {
                count--;
                if (count === 0) {
                  callback();
                }
              };
              testabilities.forEach(function (testability) {
                testability.whenStable(decrement);
              });

            }
            else { callback(); }
            """,
            rootSelector);

    private static TResult ConvertResult<TResult>(object result) =>
        AtataContext.ObjectConverter.Convert<TResult>(result);
}
