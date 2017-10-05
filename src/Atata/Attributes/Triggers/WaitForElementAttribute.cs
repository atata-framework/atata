using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Specifies the waiting for the element. By default occurs after the click.
    /// </summary>
    public class WaitForElementAttribute : WaitForAttribute
    {
        private ScopeSource? scopeSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitForElementAttribute" /> class.
        /// </summary>
        /// <param name="waitBy">The kind of the element selector to wait for.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="until">The waiting approach.</param>
        /// <param name="on">The trigger events.</param>
        /// <param name="priority">The priority.</param>
        public WaitForElementAttribute(WaitBy waitBy, string selector, WaitUntil until = WaitUntil.MissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(until, on, priority)
        {
            WaitBy = waitBy;
            Selector = selector;
        }

        /// <summary>
        /// Gets the kind of the element selector to wait for.
        /// </summary>
        public WaitBy WaitBy { get; private set; }

        /// <summary>
        /// Gets the selector.
        /// </summary>
        public string Selector { get; private set; }

        /// <summary>
        /// Gets or sets the scope source. The default value is <see cref="ScopeSource.Parent"/>.
        /// </summary>
        public ScopeSource ScopeSource
        {
            get { return scopeSource ?? ScopeSource.Parent; }
            set { scopeSource = value; }
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            WaitUnit[] waitUnits = GetWaitUnits(Until);

            foreach (WaitUnit unit in waitUnits)
                Wait(context.Component, unit);
        }

        protected virtual void Wait<TOwner>(IUIComponent<TOwner> scopeComponent, WaitUnit waitUnit)
            where TOwner : PageObject<TOwner>
        {
            ScopeSource actualScopeSource = scopeSource ?? scopeComponent.ScopeSource;

            StaleSafely.Execute(
                options =>
                {
                    IWebElement scopeElement = actualScopeSource.GetScopeElement(scopeComponent, SearchOptions.Within(options.Timeout));

                    By by = WaitBy.GetBy(Selector).With(options);

                    if (waitUnit.Method == WaitMethod.Presence)
                        scopeElement.Exists(by);
                    else
                        scopeElement.Missing(by);
                },
                waitUnit.Options);
        }
    }
}
