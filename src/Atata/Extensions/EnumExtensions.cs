using Humanizer;
using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class EnumExtensions
    {
        public static string ToTitleString(this Enum value)
        {
            return value.ToString(LetterCasing.Title);
        }

        public static string ToSentenceString(this Enum value)
        {
            return value.ToString(LetterCasing.Sentence);
        }

        public static string ToString(this Enum value, LetterCasing casing)
        {
            Type type = value.GetType();
            MemberInfo memberInfo = type.GetMember(value.ToString())[0];

            TermAttribute termAttribute = memberInfo.GetCustomAttribute<TermAttribute>(false);
            bool hasTermValue = termAttribute != null && termAttribute.Values != null && termAttribute.Values.Any();
            string term = hasTermValue ? termAttribute.Values.First() : value.ToString();

            if (hasTermValue)
                return term;
            else if (termAttribute != null && termAttribute.Format != TermFormat.Inherit)
                return termAttribute.Format.ApplyTo(term);
            else
                return term.Humanize(casing);
        }
    }
}
