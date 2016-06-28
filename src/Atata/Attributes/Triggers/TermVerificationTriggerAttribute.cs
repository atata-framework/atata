using System;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// The base trigger attribute class that can be used in the verification process when the page object is initialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public abstract class TermVerificationTriggerAttribute : TriggerAttribute, ITermSettings
    {
        protected TermVerificationTriggerAttribute(TermCase termCase)
            : this(null, termCase: termCase)
        {
        }

        protected TermVerificationTriggerAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : this(null, match, termCase)
        {
        }

        protected TermVerificationTriggerAttribute(TermMatch match, params string[] values)
            : this(values, match)
        {
        }

        protected TermVerificationTriggerAttribute(params string[] values)
            : this(values, TermMatch.Inherit)
        {
        }

        private TermVerificationTriggerAttribute(string[] values = null, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(TriggerEvents.OnPageObjectInit)
        {
            Values = values;
            Match = match;
            Case = termCase;
        }

        public string[] Values { get; private set; }
        public TermCase Case { get; private set; }
        public new TermMatch Match { get; set; }
        public string Format { get; set; }

        protected virtual TermCase DefaultFormat
        {
            get { return TermCase.Title; }
        }

        protected virtual TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
        }

        public override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            ITermSettings termSettings = ResolveTermSettings(metadata);

            Case = this.GetCaseOrNull() ?? termSettings.GetCaseOrNull() ?? DefaultFormat;
            Match = this.GetMatchOrNull() ?? termSettings.GetMatchOrNull() ?? DefaultMatch;
            Format = this.GetFormatOrNull() ?? termSettings.GetFormatOrNull();
        }

        protected virtual ITermSettings ResolveTermSettings(UIComponentMetadata metadata)
        {
            return null;
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            string[] expectedValues = GetExpectedValues(context.Component.ComponentName);

            OnExecute(context, expectedValues);
        }

        protected abstract void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
            where TOwner : PageObject<TOwner>;

        private string[] GetExpectedValues(string componentName)
        {
            if (Values != null && Values.Any())
            {
                return !string.IsNullOrEmpty(Format) ? Values.Select(x => string.Format(Format, x)).ToArray() : Values;
            }
            else
            {
                string value = Case.ApplyTo(componentName);
                if (!string.IsNullOrEmpty(Format))
                    value = string.Format(Format, value);
                return new[] { value };
            }
        }
    }
}
