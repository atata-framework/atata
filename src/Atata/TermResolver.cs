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
            string[] terms = GetTerms(value, termSettings);
            return string.Join("/", terms);
        }

        public static string[] GetTerms(object value, ITermSettings termSettings = null)
        {
            if (value is string)
                return new[] { (string)value };
            else if (value is Enum)
                return GetEnumTerms((Enum)value, termSettings);
            else
                return new[] { value.ToString() };
        }

        public static string CreateXPathCondition(object value, ITermSettings termSettings = null, string operand = ".")
        {
            string[] terms = TermResolver.GetTerms(value, termSettings);
            TermMatch match = TermResolver.GetMatch(value, termSettings);
            return match.CreateXPathCondition(terms, operand);
        }

        public static T FromString<T>(string value, ITermSettings termSettings = null)
        {
            return (T)FromString(value, typeof(T), termSettings);
        }

        public static object FromString(string value, Type destinationType, ITermSettings termSettings = null)
        {
            if (destinationType.IsEnum)
                return StringToEnum(value, destinationType, termSettings);
            else
                return Convert.ChangeType(value, destinationType);
        }

        public static object StringToEnum(string value, Type enumType, ITermSettings termSettings = null)
        {
            return Enum.GetValues(enumType).
                Cast<Enum>().
                Where(x => GetEnumMatch(x, termSettings).IsMatch(value, GetEnumTerms(x, termSettings))).
                FirstOrDefault();
        }

        public static string[] GetEnumTerms(Enum value, ITermSettings termSettings = null)
        {
            TermAttribute termAttribute = GetEnumTermAttribute(value);
            bool hasTermValue = termAttribute != null && termAttribute.Values != null && termAttribute.Values.Any();

            if (hasTermValue)
            {
                return termAttribute.Values;
            }
            else
            {
                TermFormat termFormat = GetTermFormatOrNull(termSettings)
                    ?? GetTermFormatOrNull(termAttribute)
                    ?? GetTermFormatOrNull(GetTermSettings(value.GetType()))
                    ?? DefaultFormat;

                return new[] { termFormat.ApplyTo(value.ToString()) };
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
