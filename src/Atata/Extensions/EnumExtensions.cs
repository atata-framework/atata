using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class EnumExtensions
    {
        public static Enum AddFlag(this Enum source, object flag)
        {
            try
            {
                ulong joinedValue = Convert.ToUInt64(source) | Convert.ToUInt64(flag);
                return (Enum)Enum.ToObject(source.GetType(), joinedValue);
            }
            catch (Exception exception)
            {
                throw new ArgumentException(
                    "Cannot add '{0}' value to '{1}' of enumerated type '{2}'.".FormatWith(source, flag, source.GetType().FullName),
                    exception);
            }
        }

        public static IEnumerable<Enum> GetIndividualFlags(this Enum flags)
        {
            ulong flag = 0x1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
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

        public static string ToString(this Enum value, TermFormat format)
        {
            return TermResolver.ToDisplayString(value, new TermOptions { Format = format });
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
