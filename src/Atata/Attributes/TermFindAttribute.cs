using Humanizer;
using System.Linq;

namespace Atata
{
    public abstract class TermFindAttribute : FindAttribute, ITermFindAttribute
    {
        protected TermFindAttribute(TermMatch match)
            : this(null, TermFormat.Inherit, match)
        {
        }

        protected TermFindAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : this(null, format, match)
        {
        }

        protected TermFindAttribute(string value, TermMatch match)
            : this(new[] { value }, match: match)
        {
        }

        protected TermFindAttribute(params string[] values)
            : this(values, TermFormat.Inherit)
        {
        }

        private TermFindAttribute(string[] values = null, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
        {
            Values = values;
            Format = format;
            Match = match;
            CutEnding = true;
        }

        public string[] Values { get; private set; }
        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; set; }
        public bool CutEnding { get; set; }

        public virtual string[] GetTerms(UIComponentMetadata metadata)
        {
            if (Values != null && Values.Any())
            {
                return Values;
            }
            else
            {
                TermAttribute termAttribute = metadata.GetFirstOrDefaultPropertyAttribute<TermAttribute>(x => x.Values != null && x.Values.Any());
                if (termAttribute != null)
                    return termAttribute.Values;
            }
            return new[] { GetTermFromProperty(metadata) };
        }

        private string GetTermFromProperty(UIComponentMetadata metadata)
        {
            TermFormat format = GetTermFormat(metadata);
            string name = GetPropertyName(metadata);
            return Humanize(name, format);
        }

        private TermFormat GetTermFormat(UIComponentMetadata metadata)
        {
            return Format != TermFormat.Inherit ? Format : GetTermFormatFromMetadata(metadata);
        }

        public TermMatch GetTermMatch(UIComponentMetadata metadata)
        {
            return Match != TermMatch.Inherit ? Match : GetTermMatchFromMetadata(metadata);
        }

        private string GetPropertyName(UIComponentMetadata metadata)
        {
            string name = metadata.Name;
            if (CutEnding)
            {
                string suffixToIgnore = metadata.ComponentAttribute.GetIgnoreNameEndingValues().
                    FirstOrDefault(x => name.EndsWith(x) && name.Length > x.Length);

                if (suffixToIgnore != null)
                    return name.Substring(0, name.Length - suffixToIgnore.Length).TrimEnd();
            }
            return name;
        }

        protected abstract TermFormat GetTermFormatFromMetadata(UIComponentMetadata metadata);

        protected abstract TermMatch GetTermMatchFromMetadata(UIComponentMetadata metadata);

        private static string Humanize(string name, TermFormat format)
        {
            switch (format)
            {
                case TermFormat.Title:
                    return name.Humanize(LetterCasing.Title);
                case TermFormat.Sentence:
                    return name.Humanize(LetterCasing.Sentence);
                case TermFormat.LowerCase:
                    return name.Humanize(LetterCasing.LowerCase);
                case TermFormat.UpperCase:
                    return name.Humanize(LetterCasing.AllCaps);
                case TermFormat.Camel:
                    return name.Humanize().Camelize();
                case TermFormat.Pascal:
                    return name.Humanize().Pascalize();
                case TermFormat.Dashed:
                    return name.Humanize().Dasherize();
                case TermFormat.XDashed:
                    return "x-" + name.Humanize().Dasherize();
                case TermFormat.Underscored:
                    return name.Humanize().Underscore();
                default:
                    throw ExceptionsFactory.CreateForUnsuppotedEnumValue(format, "format");
            }
        }
    }
}
