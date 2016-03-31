using Humanizer;
using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
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

        public override void Run(TriggerContext context)
        {
            string[] expectedValues = GetExpectedValues(context.Component.ComponentName);

            string matchAsString = Match.ToString(TermFormat.LowerCase);
            string expectedValuesAsString = TermResolver.ToDisplayString(expectedValues);

            context.Log.StartVerificationSection("page title {0} '{1}'", matchAsString, expectedValuesAsString);

            string actualTitle = null;
            bool containsTitle = context.Driver.Try().Until(d =>
                {
                    actualTitle = d.Title;
                    return Match.IsMatch(actualTitle, expectedValues);
                });

            if (!containsTitle)
            {
                string errorMessage = ExceptionFactory.BuildAssertionErrorMessage(
                    "String that {0} '{1}'".FormatWith(matchAsString, expectedValuesAsString),
                    string.Format("'{0}'", actualTitle),
                    "Incorrect page title");

                Assert.That(containsTitle, errorMessage);
            }
            context.Log.EndSection();
        }

        private string[] GetExpectedValues(string pageObjectName)
        {
            if (Values != null && Values.Any())
            {
                return StringFormat != null ? Values.Select(x => string.Format(StringFormat, x)).ToArray() : Values;
            }
            else
            {
                string value = Format.ApplyTo(pageObjectName);
                if (StringFormat != null)
                    value = string.Format(StringFormat, value);
                return new[] { value };
            }
        }
    }
}
