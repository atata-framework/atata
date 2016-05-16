using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class VerifyTitleAttribute : TriggerAttribute, ITermSettings
    {
        private const TermFormat DefaultFormat = TermFormat.Title;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        public VerifyTitleAttribute(TermFormat format = TermFormat.Inherit)
            : this(null, format, TermMatch.Inherit)
        {
        }

        public VerifyTitleAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : this(null, format, match)
        {
        }

        public VerifyTitleAttribute(TermMatch match, params string[] values)
            : this(values, TermFormat.Inherit, match)
        {
        }

        public VerifyTitleAttribute(params string[] values)
            : this(values, TermFormat.Inherit, TermMatch.Inherit)
        {
        }

        private VerifyTitleAttribute(string[] values = null, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(TriggerEvents.OnPageObjectInit)
        {
            Values = values;
            Format = format;
            Match = match;
            CutEnding = true;
        }

        public string[] Values { get; private set; }
        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; set; }
        public string StringFormat { get; set; }
        public bool CutEnding { get; set; }

        public override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            VerifyTitleSettingsAttribute settingsAttribute = metadata.GetFirstOrDefaultAssemblyAttribute<VerifyTitleSettingsAttribute>();

            Format = this.GetFormatOrNull() ?? settingsAttribute.GetFormatOrNull() ?? DefaultFormat;
            Match = this.GetMatchOrNull() ?? settingsAttribute.GetMatchOrNull() ?? DefaultMatch;
            StringFormat = this.GetStringFormatOrNull() ?? settingsAttribute.GetStringFormatOrNull();
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            string[] expectedValues = GetExpectedValues(context.Component.ComponentName);

            context.Component.Owner.PageTitle.VerifyUntilMatchesAny(Match, expectedValues);
        }

        private string[] GetExpectedValues(string pageObjectName)
        {
            if (Values != null && Values.Any())
            {
                return !string.IsNullOrEmpty(StringFormat) ? Values.Select(x => string.Format(StringFormat, x)).ToArray() : Values;
            }
            else
            {
                string value = Format.ApplyTo(pageObjectName);
                if (!string.IsNullOrEmpty(StringFormat))
                    value = string.Format(StringFormat, value);
                return new[] { value };
            }
        }
    }
}
