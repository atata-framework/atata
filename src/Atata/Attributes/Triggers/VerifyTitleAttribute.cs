using System;
using System.Linq;
using System.Text;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class VerifyTitleAttribute : TriggerAttribute, ITermSettings
    {
        private const TermFormat DefaultFormat = TermFormat.Title;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        public VerifyTitleAttribute(TermMatch match)
            : this(null, DefaultFormat, match)
        {
        }

        public VerifyTitleAttribute(TermFormat format, TermMatch match = DefaultMatch)
            : this(null, format, match)
        {
        }

        public VerifyTitleAttribute(string value, TermMatch match)
            : this(new[] { value }, DefaultFormat, match: match)
        {
        }

        public VerifyTitleAttribute()
            : this(null, DefaultFormat)
        {
        }

        public VerifyTitleAttribute(params string[] values)
            : this(values, DefaultFormat)
        {
        }

        private VerifyTitleAttribute(string[] values = null, TermFormat format = DefaultFormat, TermMatch match = DefaultMatch)
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

        public override void Run(TriggerContext context)
        {
            string[] expectedValues = GetExpectedValues(context.Component.ComponentName);

            string actualTitle = null;
            bool containsTitle = context.Driver.Try().Until(d =>
                {
                    actualTitle = d.Title;
                    return Match.IsMatch(actualTitle, expectedValues);
                });

            if (!containsTitle)
            {
                string message = new StringBuilder().
                    Append("Incorrect page title.").
                    AppendLine().
                    AppendFormat("Expected: String that {0} '{1}'",
                        TermResolver.ToDisplayString(Match, new TermOptions { Format = TermFormat.LowerCase }),
                        TermResolver.ToDisplayString(expectedValues)).
                    AppendLine().
                    AppendFormat("But was: '{0}'", actualTitle).ToString();

                Assert.That(containsTitle, message);
            }
        }

        private string[] GetExpectedValues(string pageObjectName)
        {
            if (Values != null)
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
