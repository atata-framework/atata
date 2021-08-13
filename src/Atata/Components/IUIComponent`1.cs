using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents an interface for UI component.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public interface IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the owner page object.
        /// </summary>
        TOwner Owner { get; }

        /// <summary>
        /// Gets the parent component.
        /// </summary>
        IUIComponent<TOwner> Parent { get; }

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the control is visible.
        /// </summary>
        DataProvider<bool, TOwner> IsVisible { get; }

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the text content.
        /// </summary>
        DataProvider<string, TOwner> Content { get; }

        /// <summary>
        /// Gets the source of the scope.
        /// </summary>
        ScopeSource ScopeSource { get; }

        /// <summary>
        /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element associated with this component.
        /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        /// <exception cref="NoSuchElementException">Element not found.</exception>
        IWebElement Scope { get; }

        /// <summary>
        /// Gets the <see cref="ISearchContext"/> instance that represents the scope search context
        /// (where to find the children of this component).
        /// By default is the same as <see cref="Scope"/>.
        /// Also can execute <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        ISearchContext ScopeContext { get; }

        /// <summary>
        /// Gets the scope locator.
        /// </summary>
        IScopeLocator ScopeLocator { get; }

        /// <summary>
        /// Gets or sets the name of the component.
        /// </summary>
        string ComponentName { get; set; }

        /// <summary>
        /// Gets the name of the component type.
        /// Returns the value of <see cref="UIComponentDefinitionAttribute.ComponentTypeName"/> property for control from <see cref="ControlDefinitionAttribute"/>
        /// and for page object from <see cref="PageObjectDefinitionAttribute"/>.
        /// </summary>
        string ComponentTypeName { get; }

        /// <summary>
        /// Gets the full name of the component including parent component full name, own component name and own component type name.
        /// </summary>
        string ComponentFullName { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentLocationProvider{TOwner}"/> instance that provides an access to the scope element's location (X and Y).
        /// </summary>
        UIComponentLocationProvider<TOwner> ComponentLocation { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentSizeProvider{TOwner}"/> instance that provides an access to the scope element's size (Width and Height).
        /// </summary>
        UIComponentSizeProvider<TOwner> ComponentSize { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentAttributeProvider{TOwner}"/> instance that provides an access to the scope element's attributes.
        /// </summary>
        UIComponentAttributeProvider<TOwner> Attributes { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentCssProvider{TOwner}"/> instance that provides an access to the scope element's CSS properties.
        /// </summary>
        UIComponentCssProvider<TOwner> Css { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentScriptExecutor{TOwner}"/> instance that provides a set of methods for JavaScript execution.
        /// </summary>
        UIComponentScriptExecutor<TOwner> Script { get; }

        /// <summary>
        /// Gets the list of child controls.
        /// </summary>
        UIComponentChildrenList<TOwner> Controls { get; }

        /// <summary>
        /// Gets the metadata of the component.
        /// </summary>
        UIComponentMetadata Metadata { get; }

        /// <summary>
        /// Gets the set of triggers.
        /// Provides the functionality to get/add/remove triggers dynamically.
        /// </summary>
        UIComponentTriggerSet<TOwner> Triggers { get; }

        /// <summary>
        /// Gets the verification provider that provides a set of verification extension methods.
        /// </summary>
        UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should { get; }

        /// <summary>
        /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element.
        /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        /// <param name="options">The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns>The <see cref="IWebElement"/> instance of the scope.</returns>
        IWebElement GetScope(SearchOptions options = null);

        /// <summary>
        /// Gets the <see cref="ISearchContext"/> instance that represents the scope search context
        /// (where to find the children of this component).
        /// Also can execute <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        /// <param name="options">
        /// The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns>The <see cref="ISearchContext"/> instance of the scope context.</returns>
        ISearchContext GetScopeContext(SearchOptions options = null);

        /// <summary>
        /// Waits until the specified component condition is met.
        /// </summary>
        /// <param name="until">The waiting condition.</param>
        /// <param name="options">The options.</param>
        /// <returns>The instance of the owner page object.</returns>
        TOwner Wait(Until until, WaitOptions options = null);

        /// <summary>
        /// Determines whether the component exists.
        /// </summary>
        /// <param name="options">The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns><see langword="true"/> if the component exists; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="NoSuchElementException">
        /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
        /// equal to <see langword="false"/> value and the component doesn't exist.
        /// </exception>
        bool Exists(SearchOptions options = null);

        /// <summary>
        /// Determines whether the component is missing.
        /// </summary>
        /// <param name="options">The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns><see langword="true"/> if the component is missing; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="NotMissingElementException">
        /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
        /// equal to <see langword="false"/> value and the component exists.
        /// </exception>
        bool Missing(SearchOptions options = null);

        /// <summary>
        /// Gets the data provider by name or creates and stores a new instance with the specified <paramref name="providerName"/> and using <paramref name="valueGetFunction"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="valueGetFunction">The function that gets a value.</param>
        /// <returns>A new instance of <see cref="DataProvider{TData, TOwner}"/> type or already stored one.</returns>
        DataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction);

        /// <summary>
        /// Gets the ancestor component of specified type.
        /// </summary>
        /// <typeparam name="TComponentToFind">The type of the component to find.</typeparam>
        /// <returns>The component or <see langword="null"/> if not found.</returns>
        TComponentToFind GetAncestor<TComponentToFind>()
            where TComponentToFind : UIComponent<TOwner>;

        /// <summary>
        /// Gets the ancestor component of specified type or self.
        /// </summary>
        /// <typeparam name="TComponentToFind">The type of the component to find.</typeparam>
        /// <returns>The component or <see langword="null"/> if not found.</returns>
        TComponentToFind GetAncestorOrSelf<TComponentToFind>()
            where TComponentToFind : UIComponent<TOwner>;

        /// <summary>
        /// Gets a behavior attribute from the component's metadata and then invokes
        /// the specified <paramref name="behaviorExecutionAction" /> with passing the found behavior to it.
        /// </summary>
        /// <typeparam name="TBehaviorAttribute">The type of the behavior attribute.</typeparam>
        /// <param name="behaviorExecutionAction">The behavior execution action.</param>
        void ExecuteBehavior<TBehaviorAttribute>(Action<TBehaviorAttribute> behaviorExecutionAction)
            where TBehaviorAttribute : MulticastAttribute;

        /// <summary>
        /// Gets a behavior attribute from the component's metadata and then invokes
        /// the specified <paramref name="behaviorExecutionFunction" /> with passing the found behavior to it.
        /// </summary>
        /// <typeparam name="TBehaviorAttribute">The type of the behavior attribute.</typeparam>
        /// <typeparam name="TResult">The type of the behavior result.</typeparam>
        /// <param name="behaviorExecutionFunction">The behavior execution function.</param>
        /// <returns>The result of behavior execution.</returns>
        TResult ExecuteBehavior<TBehaviorAttribute, TResult>(Func<TBehaviorAttribute, TResult> behaviorExecutionFunction)
            where TBehaviorAttribute : MulticastAttribute;
    }
}
