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
            string term = (termAttribute != null && termAttribute.Values != null && termAttribute.Values.Any())
                ? termAttribute.Values.First()
                : value.ToString();

            return (termAttribute.Format != TermFormat.Inherit) ? termAttribute.Format.ApplyTo(term) : term.Humanize(casing);
        }
    }
}
