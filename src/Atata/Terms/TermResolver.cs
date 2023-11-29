namespace Atata;

public static class TermResolver
{
    private const TermCase DefaultCase = TermCase.Title;

    private const TermMatch DefaultMatch = TermMatch.Equals;

    private static readonly Dictionary<Type, TermConverter> s_typeTermConverters = [];

    static TermResolver() =>
        RegisterStandardConverters();

    private static void RegisterStandardConverters()
    {
        RegisterNumberConverter(sbyte.Parse);
        RegisterNumberConverter(byte.Parse);
        RegisterNumberConverter(short.Parse);
        RegisterNumberConverter(ushort.Parse);
        RegisterNumberConverter(int.Parse);
        RegisterNumberConverter(uint.Parse);
        RegisterNumberConverter(long.Parse);
        RegisterNumberConverter(ulong.Parse);
        RegisterNumberConverter(float.Parse);
        RegisterNumberConverter(double.Parse);
        RegisterNumberConverter(decimal.Parse);

        RegisterConverter<DateTime>(
            (s, opt) =>
            {
                string stringValue = RetrieveValueFromString(s, opt.Format);
                string specificFormat = RetrieveSpecificFormatFromStringFormat(opt.Format);

                return specificFormat == null
                    ? DateTime.Parse(stringValue, opt.Culture)
                    : DateTime.ParseExact(stringValue, specificFormat, opt.Culture);
            });

        RegisterConverter<TimeSpan>(
            (s, opt) =>
            {
                string stringValue = RetrieveValueFromString(s, opt.Format);
                string specificFormat = RetrieveSpecificFormatFromStringFormat(opt.Format);

                if (specificFormat == null)
                    return TimeSpan.Parse(stringValue, opt.Culture);
                else if (specificFormat.Contains("t"))
                    return DateTime.ParseExact(stringValue, specificFormat, opt.Culture).TimeOfDay;
                else
                    return TimeSpan.ParseExact(stringValue, specificFormat, opt.Culture);
            },
            (v, opt) =>
            {
                string specificFormat = RetrieveSpecificFormatFromStringFormat(opt.Format);

                return specificFormat != null && specificFormat.Contains("t")
                    ? FormatValue(
                        DateTime.Today.Add(v).ToString(specificFormat, opt.Culture),
                        opt.Format,
                        opt.Culture)
                    : FormatValue(v, opt.Format, opt.Culture);
            });

        RegisterConverter<Guid>(
            (s, opt) =>
            {
                string stringValue = RetrieveValueFromString(s, opt.Format);
                string specificFormat = RetrieveSpecificFormatFromStringFormat(opt.Format);

                return specificFormat == null
                    ? Guid.Parse(stringValue)
                    : Guid.ParseExact(stringValue, specificFormat);
            });
    }

    private static void RegisterNumberConverter<T>(
        Func<string, NumberStyles, IFormatProvider, T> parseFunction)
        where T : IFormattable
        =>
        RegisterConverter(
            typeof(T),
            (s, opt) =>
            {
                string stringValue = RetrieveValueFromString(s, opt.Format);
                string specificFormat = RetrieveSpecificFormatFromStringFormat(opt.Format);

                bool isPercentageFormat = specificFormat != null && specificFormat.StartsWith("P", StringComparison.InvariantCultureIgnoreCase);

                if (isPercentageFormat)
                {
                    stringValue = stringValue
                        .Replace(opt.Culture.NumberFormat.PercentSymbol, string.Empty)
                        .Replace(opt.Culture.NumberFormat.PercentDecimalSeparator, opt.Culture.NumberFormat.NumberDecimalSeparator);

                    decimal percent = decimal.Parse(stringValue, NumberStyles.Any, opt.Culture) / 100;
                    return Convert.ChangeType(percent, typeof(T), opt.Culture);
                }
                else
                {
                    return parseFunction(stringValue, NumberStyles.Any, opt.Culture);
                }
            });

    public static void RegisterConverter<T>(
        Func<string, TermOptions, T> fromStringConverter,
        Func<T, TermOptions, string> toStringConverter = null)
    {
        fromStringConverter.CheckNotNull(nameof(fromStringConverter));

        object CastedFromStringConverter(string s, TermOptions to) =>
            fromStringConverter(s, to);

        Func<object, TermOptions, string> castedToStringConverter = null;

        if (toStringConverter != null)
            castedToStringConverter = (v, to) => toStringConverter((T)v, to);

        RegisterConverter(typeof(T), CastedFromStringConverter, castedToStringConverter);
    }

    public static void RegisterConverter(
        Type type,
        Func<string, TermOptions, object> fromStringConverter,
        Func<object, TermOptions, string> toStringConverter = null)
    {
        fromStringConverter.CheckNotNull(nameof(fromStringConverter));

        s_typeTermConverters[type] = new TermConverter
        {
            FromStringConverter = fromStringConverter,
            ToStringConverter = toStringConverter
        };
    }

    public static string ToDisplayString(object value, TermOptions termOptions = null) =>
        value is IEnumerable<object> enumerable
            ? string.Join("/", enumerable.Select(x => ToDisplayString(x, termOptions)))
            : ToString(value, termOptions);

    public static string ToString(object value, TermOptions termOptions = null)
    {
        if (value == null || Equals(value, string.Empty))
            return value as string;

        string[] terms = GetTerms(value, termOptions);
        return string.Join("/", terms);
    }

    public static string[] GetTerms(object value, TermOptions termOptions = null)
    {
        value.CheckNotNull(nameof(value));

        termOptions ??= new TermOptions();

        if (value is string stringValue)
            return [FormatStringValue(stringValue, termOptions)];
        else if (value is Enum enumValue)
            return GetEnumTerms(enumValue, termOptions);
        else if (s_typeTermConverters.TryGetValue(value.GetType(), out TermConverter termConverter) && termConverter.ToStringConverter != null)
            return [termConverter.ToStringConverter(value, termOptions)];
        else
            return [FormatValue(value, termOptions.Format, termOptions.Culture)];
    }

    private static string FormatStringValue(string value, TermOptions termOptions)
    {
        string valueToFormat = termOptions.GetCaseOrNull()?.ApplyTo(value) ?? value;

        return FormatValue(valueToFormat, termOptions.Format, termOptions.Culture);
    }

    private static string FormatValue(object value, string format, CultureInfo culture)
    {
        if (IsComplexStringFormat(format))
            return string.Format(culture, format, value);
        else if (value is IFormattable formattableValue)
            return formattableValue.ToString(format, culture);
        else
            return value?.ToString();
    }

    private static bool IsComplexStringFormat(string format) =>
        format != null && format.Contains("{0");

    private static string RetrieveValueFromString(string value, string format) =>
        IsComplexStringFormat(format) ? RetrieveValuePart(value, format) : value;

    private static string RetrieveSpecificFormatFromStringFormat(string format)
    {
        if (string.IsNullOrEmpty(format))
        {
            return null;
        }
        else if (IsComplexStringFormat(format))
        {
            int startIndex = format.IndexOf("{0", StringComparison.Ordinal);
            int endIndex = format.IndexOf('}', startIndex + 2);

            return endIndex - startIndex == 2
                ? null
                : format.Substring(startIndex + 3, endIndex - startIndex - 3);
        }
        else
        {
            return format;
        }
    }

    private static string RetrieveValuePart(string value, string format)
    {
        if (string.IsNullOrEmpty(format))
            return value;

        string[] formatParts = format.Split(new[] { "{0" }, 2, StringSplitOptions.None);

        if (formatParts.Length != 2)
        {
            throw new ArgumentException(
                $"Incorrect \"{format}\" {nameof(format)} for \"{value}\" string. Format should match regular expression: \".*{{0}}.*\".",
                nameof(format));
        }

        formatParts[1] = formatParts[1].Substring(formatParts[1].IndexOf('}') + 1);

        string formatStart = ReplaceDoubleCurlyBracesWithSingleOnes(formatParts[0]);
        string formatEnd = ReplaceDoubleCurlyBracesWithSingleOnes(formatParts[1]);

        if (!value.StartsWith(formatStart, StringComparison.Ordinal))
        {
            throw new ArgumentException(
                $"\"{value}\" value doesn't match the \"{format}\" {nameof(format)}. Should start with \"{formatStart}\".",
                nameof(value));
        }

        if (!value.EndsWith(formatEnd, StringComparison.Ordinal))
        {
            throw new ArgumentException(
                $"\"{value}\" value doesn't match the \"{format}\" {nameof(format)}. Should end with \"{formatEnd}\".",
                nameof(value));
        }

        return value.Substring(formatStart.Length, value.Length - formatStart.Length - formatEnd.Length);
    }

    private static string ReplaceDoubleCurlyBracesWithSingleOnes(string value) =>
        value.Replace("{{", "{").Replace("}}", "}");

    public static string CreateXPathCondition(object value, TermOptions termOptions = null, string operand = ".")
    {
        string[] terms = GetTerms(value, termOptions);
        TermMatch match = GetMatch(value, termOptions);
        return match.CreateXPathCondition(terms, operand);
    }

    public static T FromString<T>(string value, TermOptions termOptions = null)
    {
        object result = FromString(value, typeof(T), termOptions);
        return (T)result;
    }

    public static object FromString(string value, Type destinationType, TermOptions termOptions = null)
    {
        object result = value is null
            ? null
            : RetrieveValueFromString(value, destinationType, termOptions ?? new TermOptions());

        if (result == null && !destinationType.IsClassOrNullable())
        {
            throw new ArgumentException(
                "Failed to find value of type '{0}' corresponding to '{1}'.".FormatWith(destinationType.FullName, value),
                nameof(value));
        }
        else
        {
            return result;
        }
    }

    private static object RetrieveValueFromString(string value, Type destinationType, TermOptions termOptions)
    {
        Type underlyingType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

        if (underlyingType.IsEnum)
        {
            return StringToEnum(value, underlyingType, termOptions);
        }
        else if (!string.IsNullOrEmpty(value))
        {
            return s_typeTermConverters.TryGetValue(underlyingType, out TermConverter termConverter)
                ? termConverter.FromStringConverter(value, termOptions)
                : Convert.ChangeType(RetrieveValuePart(value, termOptions.Format), underlyingType, termOptions.Culture);
        }
        else
        {
            return underlyingType == typeof(string) ? value : null;
        }
    }

    public static object StringToEnum(string value, Type enumType, TermOptions termOptions = null) =>
        enumType.GetIndividualEnumFlags()
            .FirstOrDefault(x => GetEnumMatch(x, termOptions).IsMatch(value, GetEnumTerms(x, termOptions)));

    public static string[] GetEnumTerms(Enum value, TermOptions termOptions = null)
    {
        termOptions ??= new TermOptions();

        return value.GetType().IsDefined(typeof(FlagsAttribute), false)
            ? GetFlagsEnumTerms(value, termOptions)
            : GetIndividualEnumTerms(value, termOptions);
    }

    private static string[] GetFlagsEnumTerms(Enum value, TermOptions termOptions) =>
        value.GetIndividualFlags()
            .SelectMany(x => GetIndividualEnumTerms(x, termOptions))
            .ToArray();

    private static string[] GetIndividualEnumTerms(Enum value, TermOptions termOptions)
    {
        TermAttribute termAttribute = GetEnumTermAttribute(value);
        ITermSettings termSettings = GetTermSettingsAttribute(value.GetType());

        TermCase? termCase = termOptions.GetCaseOrNull();
        string termFormat = termOptions.GetFormatOrNull();

        if (termAttribute != null || termSettings != null)
        {
            string[] terms = GetIndividualEnumTerms(value, termAttribute, termSettings, termOptions.Culture);

            if (termCase.HasValue)
                terms = terms.Select(x => ApplyCaseWithoutWordBreak(x, termCase.Value)).ToArray();

            return terms.Select(x => FormatValue(x, termFormat, termOptions.Culture)).ToArray();
        }
        else if (termCase == null && (termFormat != null && !termFormat.Contains("{0}")))
        {
            return [FormatValue(value, termFormat, termOptions.Culture)];
        }
        else
        {
            string term = TermCaseResolver.ApplyCase(value.ToString(), termCase ?? DefaultCase);
            return [FormatValue(term, termFormat, termOptions.Culture)];
        }
    }

    private static string[] GetIndividualEnumTerms(Enum value, TermAttribute termAttribute, ITermSettings termSettings, CultureInfo culture)
    {
        string[] values = termAttribute?.Values?.Any() ?? false
            ? termAttribute.Values
            : [
                TermCaseResolver.ApplyCase(
                    value.ToString(),
                    termAttribute.GetCaseOrNull() ?? termSettings.GetCaseOrNull() ?? DefaultCase)
            ];

        string termFormat = termAttribute.GetFormatOrNull() ?? termSettings.GetFormatOrNull();

        return termFormat != null
            ? values.Select(x => FormatValue(x, termFormat, culture)).ToArray()
            : values;
    }

    private static string ApplyCaseWithoutWordBreak(string value, TermCase termCase)
    {
        string[] words = value.Split(' ');
        return TermCaseResolver.ApplyCase(words, termCase);
    }

    public static TermMatch GetMatch(object value, ITermSettings termSettings = null) =>
        value is Enum enumValue
            ? GetEnumMatch(enumValue, termSettings)
            : termSettings.GetMatchOrNull() ?? DefaultMatch;

    public static TermMatch GetEnumMatch(Enum value, ITermSettings termSettings = null) =>
        termSettings.GetMatchOrNull()
            ?? GetEnumTermAttribute(value).GetMatchOrNull()
            ?? GetTermSettingsAttribute(value.GetType()).GetMatchOrNull()
            ?? DefaultMatch;

    private static TermAttribute GetEnumTermAttribute(Enum value)
    {
        Type type = value.GetType();
        MemberInfo memberInfo = type.GetMember(value.ToString())[0];

        return memberInfo.GetCustomAttribute<TermAttribute>(false);
    }

    private static TermSettingsAttribute GetTermSettingsAttribute(Type type) =>
        type.GetCustomAttribute<TermSettingsAttribute>(false);

    private sealed class TermConverter
    {
        public Func<string, TermOptions, object> FromStringConverter { get; set; }

        public Func<object, TermOptions, string> ToStringConverter { get; set; }
    }
}
