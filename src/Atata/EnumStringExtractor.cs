using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class EnumStringExtractor
    {
        public static string[] Extract(Enum enumValue)
        {
            Type type = enumValue.GetType();
            MemberInfo memberInfo = type.GetMember(enumValue.ToString())[0];
            TermAttribute attribute = memberInfo.GetCustomAttribute<TermAttribute>();
            return attribute != null ? attribute.Values : new string[0];
        }

        public static bool TryExtract(Enum enumValue, out string[] values)
        {
            values = Extract(enumValue);
            return values != null && values.Any();
        }
    }
}
