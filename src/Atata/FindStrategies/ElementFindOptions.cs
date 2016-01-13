using Humanizer;
using System;

namespace Atata
{
    public class ElementFindOptions : ICloneable
    {
        public string[] Qualifiers { get; set; }
        public string ElementXPath { get; set; }
        public string IdFinderFormat { get; set; }
        public int? Index { get; set; }
        public QualifierMatch Match { get; set; }
        public bool IsSafely { get; set; }

        public bool HasIndex
        {
            get { return Index.HasValue; }
        }

        public int? Position
        {
            get { return HasIndex ? Index + 1 : null; }
        }

        public string GetQualifiersXPathCondition(string value = ".")
        {
            return Match.CreateXPathCondition(Qualifiers, value);
        }

        public string GetQualifiersAsString()
        {
            return Qualifiers != null ? string.Join("/", Qualifiers) : null;
        }

        public string GetPositionWrappedXPathCondition()
        {
            return HasIndex ? "[{0}]".FormatWith(Position) : null;
        }

        public ElementFindOptions Clone()
        {
            return (ElementFindOptions)MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
    }
}
