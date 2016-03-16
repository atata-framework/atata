using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class TermResolver
    {
        private const TermFormat DefaultFormat = TermFormat.Title;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        public static string ToString(object value, TermOptions termOptions = null)
        {
            if (value == null)
                return "null";

            string[] terms = GetTerms(value, termOptions);
            return string.Join("/", terms);
        }

        public static string[] GetTerms(object value, TermOptions termOptions = null)
        {
            if (value is string)
                return new[] { (string)value };
            else if (value is Enum)
                return GetEnumTerms((Enum)value, termOptions);
            else
                return new[] { value.ToString() };
        }

        public static string CreateXPathCondition(object value, TermOptions termOptions = null, string operand = ".")
        {
            string[] terms = GetTerms(value, termOptions);
            TermMatch match = GetMatch(value, termOptions);
            return match.CreateXPathCondition(terms, operand);
        }

        public static T FromString<T>(string value, TermOptions termOptions = null)
        {
            return (T)FromString(value, typeof(T), termOptions);
        }

        public static object FromString(string value, Type destinationType, TermOptions termOptions = null)
        {
            destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

            if (destinationType.IsEnum)
                return StringToEnum(value, destinationType, termOptions);
            else
                return Convert.ChangeType(value, destinationType);
        }

        public static object StringToEnum(string value, Type enumType, TermOptions termOptions = null)
        {
            return Enum.GetValues(enumType).
                Cast<Enum>().
                Where(x => GetEnumMatch(x, termOptions).IsMatch(value, GetEnumTerms(x, termOptions))).
                FirstOrDefault();
        }

        public static string[] GetEnumTerms(Enum value, TermOptions termOptions = null)
        {
            TermAttribute termAttribute = GetEnumTermAttribute(value);
            bool hasTermValue = termAttribute != null && termAttribute.Values != null && termAttribute.Values.Any();

            if (hasTermValue)
            {
                return termAttribute.Values;
            }
            else
            {
                TermFormat termFormat = GetTermFormatOrNull(termOptions)
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
                return GetTermMatchOrNull(termSettings) ?? DefaultMatch;
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
