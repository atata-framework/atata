using Humanizer;
using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class TermConverter
    {
        public static string ToString(object value)
        {
            if (value is string)
                return (string)value;
            else if (value is Enum)
                return EnumToString((Enum)value);
            else
                return value.ToString();
        }

        public static string EnumToString(Enum value)
        {
            Type type = value.GetType();
            MemberInfo memberInfo = type.GetMember(value.ToString())[0];

            TermAttribute termAttribute = memberInfo.GetCustomAttribute<TermAttribute>(false);
            bool hasTermValue = termAttribute != null && termAttribute.Values != null && termAttribute.Values.Any();
            string term = hasTermValue ? termAttribute.Values.First() : value.ToString();

            if (hasTermValue)
            {
                return term;
            }
            else if (termAttribute != null && termAttribute.Format != TermFormat.Inherit)
            {
                return termAttribute.Format.ApplyTo(term);
            }
            else
            {
                return term.Humanize(LetterCasing.Title);
            }
        }
    }
}
