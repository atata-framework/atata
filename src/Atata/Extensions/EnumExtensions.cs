using Humanizer;
using System;
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

            NameAttribute nameAttribute = memberInfo.GetCustomAttribute<NameAttribute>(false);
            if (nameAttribute != null)
                return nameAttribute.Value;
            else
                return value.Humanize(casing);
        }
    }
}
