using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class TermResolver
    {
        private const TermFormat DefaultFormat = TermFormat.Title;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        private static readonly Dictionary<Type, TermConverter> TypeTermConverters;

        static TermResolver()
        {
            TypeTermConverters = new Dictionary<Type, TermConverter>();

            RegisterStandardConverters();
        }

        private static void RegisterStandardConverters()
        {
            RegisterNumericConverter(sbyte.Parse);
            RegisterNumericConverter(byte.Parse);
            RegisterNumericConverter(short.Parse);
            RegisterNumericConverter(ushort.Parse);
            RegisterNumericConverter(int.Parse);
            RegisterNumericConverter(uint.Parse);
            RegisterNumericConverter(long.Parse);
            RegisterNumericConverter(ulong.Parse);
            RegisterNumericConverter(float.Parse);
            RegisterNumericConverter(double.Parse);
            RegisterNumericConverter(decimal.Parse);
        }

        private static void RegisterNumericConverter<T>(Func<string, NumberStyles, IFormatProvider, T> fromStringConverter)
            where T : IFormattable
        {
            RegisterConverter(
                typeof(T),
                (s, to) => fromStringConverter(s, NumberStyles.Any, to.Culture),
                (v, to) => ((T)v).ToString(to.StringFormat, to.Culture));
        }

        public static void RegisterConverter<T>(
            Func<string, TermOptions, T> fromStringConverter,
            Func<T, TermOptions, string> toStringConverter = null)
        {
            if (fromStringConverter == null)
                throw new ArgumentNullException("fromStringConverter");

            Func<string, TermOptions, object> castedFromStringConverter = (s, to) => fromStringConverter(s, to);
            Func<object, TermOptions, string> castedToStringConverter = null;
            if (toStringConverter != null)
                castedToStringConverter = (v, to) => toStringConverter((T)v, to);

            RegisterConverter(typeof(T), castedFromStringConverter, castedToStringConverter);
        }

        public static void RegisterConverter(
            Type type,
            Func<string, TermOptions, object> fromStringConverter,
            Func<object, TermOptions, string> toStringConverter = null)
        {
            if (fromStringConverter == null)
                throw new ArgumentNullException("fromStringConverter");

            TypeTermConverters[type] = new TermConverter
            {
                FromStringConverter = fromStringConverter,
                ToStringConverter = toStringConverter
            };
        }

        public static string ToString(object value, TermOptions termOptions = null)
        {
            if (value == null)
                return "null";

            string[] terms = GetTerms(value, termOptions);
            return string.Join("/", terms);
        }

        public static string[] GetTerms(object value, TermOptions termOptions = null)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            termOptions = termOptions ?? TermOptions.CreateDefault();
            TermConverter termConverter;

            if (value is string)
                return new[] { (string)value };
            else if (value is Enum)
                return GetEnumTerms((Enum)value, termOptions);
            else if (TypeTermConverters.TryGetValue(value.GetType(), out termConverter) && termConverter.ToStringConverter != null)
                return new[] { termConverter.ToStringConverter(value, termOptions) };
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
            termOptions = termOptions ?? TermOptions.CreateDefault();
            destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
            TermConverter termConverter;

            if (destinationType.IsEnum)
                return StringToEnum(value, destinationType, termOptions);
            else if (TypeTermConverters.TryGetValue(destinationType, out termConverter))
                return termConverter.FromStringConverter(value, termOptions);
            else
                return Convert.ChangeType(value, destinationType, termOptions.Culture);
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

        private class TermConverter
        {
            public Func<string, TermOptions, object> FromStringConverter { get; set; }
            public Func<object, TermOptions, string> ToStringConverter { get; set; }
        }
    }
}
