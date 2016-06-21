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
        protected TermVerificationTriggerAttribute(TermFormat format)
            : this(null, format: format)
        {
        }

        protected TermVerificationTriggerAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : this(null, match, format)
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

        private TermVerificationTriggerAttribute(string[] values = null, TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(TriggerEvents.OnPageObjectInit)
        {
            Values = values;
            Match = match;
            Format = format;
        }

        public string[] Values { get; private set; }
        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; set; }
        public string StringFormat { get; set; }

        protected virtual TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        protected virtual TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
        }

        public override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            ITermSettings termSettings = ResolveTermSettings(metadata);

            Format = this.GetFormatOrNull() ?? termSettings.GetFormatOrNull() ?? DefaultFormat;
            Match = this.GetMatchOrNull() ?? termSettings.GetMatchOrNull() ?? DefaultMatch;
            StringFormat = this.GetStringFormatOrNull() ?? termSettings.GetStringFormatOrNull();
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
                return !string.IsNullOrEmpty(StringFormat) ? Values.Select(x => string.Format(StringFormat, x)).ToArray() : Values;
            }
            else
            {
                string value = Format.ApplyTo(componentName);
                if (!string.IsNullOrEmpty(StringFormat))
                    value = string.Format(StringFormat, value);
                return new[] { value };
            }
        }
    }
}
