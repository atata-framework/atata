using Humanizer;
using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class EnumExtensions
    {
        public static Enum AddFlag(this Enum source, Enum flagToAdd)
        {
            try
            {
                return (Enum)(object)((ulong)(object)source | (ulong)(object)flagToAdd);
            }
            catch (Exception exception)
            {
                throw new ArgumentException(
                    "Cannot add '{0}' value to '{1}' of enumerated type '{2}'.".FormatWith(source, flagToAdd, source.GetType().FullName),
                    exception);
            }
        }

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
