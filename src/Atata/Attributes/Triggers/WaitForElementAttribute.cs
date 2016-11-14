using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Specifies the waiting for the element. By default occurs after the click.
    /// </summary>
    public class WaitForElementAttribute : WaitForAttribute
    {
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
        /// Gets or sets the scope source.
        /// </summary>
        public ScopeSource ScopeSource { get; set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            ScopeSource scopeSource = ScopeSource != ScopeSource.Inherit ? ScopeSource : context.Component.ScopeSource;
            IWebElement scopeElement = scopeSource.GetScopeElement((UIComponent)context.Component);

            WaitUnit[] waitUnits = GetWaitUnits(Until);

            Wait(scopeElement, waitUnits);
        }

        protected virtual void Wait(IWebElement scopeElement, WaitUnit[] waitUnits)
        {
            foreach (WaitUnit unit in waitUnits)
            {
                By by = WaitBy.GetBy(Selector).With(unit.Options);

                if (unit.Method == WaitMethod.Presence)
                    scopeElement.Exists(by);
                else
                    scopeElement.Missing(by);
            }
        }
    }
}
