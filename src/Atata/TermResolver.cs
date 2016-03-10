using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class TermResolver
    {
        private const TermFormat DefaultFormat = TermFormat.Title;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        public static string ToString(object value, ITermSettings termSettings = null)
        {
            if (value is string)
                return (string)value;
            else if (value is Enum)
                return EnumToString((Enum)value, termSettings);
            else
                return value.ToString();
        }

        public static string EnumToString(Enum value, ITermSettings termSettings = null)
        {
            TermAttribute termAttribute = GetEnumTermAttribute(value);
            bool hasTermValue = termAttribute != null && termAttribute.Values != null && termAttribute.Values.Any();
            string term = hasTermValue ? termAttribute.Values.First() : value.ToString();

            if (hasTermValue)
            {
                return term;
            }
            else
            {
                TermFormat termFormat = GetTermFormatOrNull(termSettings)
                    ?? GetTermFormatOrNull(termAttribute)
                    ?? GetTermFormatOrNull(GetTermSettings(value.GetType()))
                    ?? DefaultFormat;

                return termFormat.ApplyTo(term);
            }
        }

        public static TermMatch GetMatch(object value, ITermSettings termSettings = null)
        {
            if (value is Enum)
                return GetEnumMatch((Enum)value, termSettings);
            else
                return GetTermMatchOrNull(termSettings) ?? TermMatch.Equals;
        }

        public static TermMatch GetEnumMatch(Enum value, ITermSettings termSettings = null)
        {
            return GetTermMatchOrNull(termSettings)
                ?? GetTermMatchOrNull(GetEnumTermAttribute(value))
                ?? GetTermMatchOrNull(GetTermSettings(value.GetType()))
                ?? DefaultMatch;
        }

        private static TermAttribute GetEnumTermAttribute(Enum value)
        {
            Type type = value.GetType();
            MemberInfo memberInfo = type.GetMember(value.ToString())[0];

            return memberInfo.GetCustomAttribute<TermAttribute>(false);
        }

        private static ITermSettings GetTermSettings(Type type)
        {
            return type.GetCustomAttribute<TermSettingsAttribute>(false);
        }

        private static TermFormat? GetTermFormatOrNull(ITermSettings termSettings)
        {
            return termSettings != null && termSettings.Format != TermFormat.Inherit
                ? termSettings.Format
                : (TermFormat?)null;
        }

        private static TermMatch? GetTermMatchOrNull(ITermSettings termSettings)
        {
            return termSettings != null && termSettings.Match != TermMatch.Inherit
                ? termSettings.Match
                : (TermMatch?)null;
        }
    }
}
