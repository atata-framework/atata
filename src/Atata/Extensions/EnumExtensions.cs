#nullable enable

namespace Atata;

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
        Type dataType = Enum.GetUnderlyingType(flags.GetType());

        return dataType == typeof(int) || dataType == typeof(long) || dataType == typeof(short) || dataType == typeof(sbyte)
            ? GetIndividualFlagsOfSignedNumberType(flags)
            : GetIndividualFlagsOfUnsignedNumberType(flags);
    }

    private static IEnumerable<Enum> GetIndividualFlagsOfSignedNumberType(this Enum flags)
    {
        long flag = 0x1;

        foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
        {
            long bits = Convert.ToInt64(value);

            while (flag > 0 && flag < bits)
                flag <<= 1;

            if (flag == bits && flags.HasFlag(value))
                yield return value;
        }
    }

    private static IEnumerable<Enum> GetIndividualFlagsOfUnsignedNumberType(this Enum flags)
    {
        ulong flag = 0x1;

        foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
        {
            ulong bits = Convert.ToUInt64(value);

            while (flag > 0 && flag < bits)
                flag <<= 1;

            if (flag == bits && flags.HasFlag(value))
                yield return value;
        }
    }

    internal static string ToExpressionValueString(this Enum value, bool wrapCombinationalValueWithParentheses = false)
    {
        string[] valueStringParts = value.ToString().Split([", "], StringSplitOptions.RemoveEmptyEntries);
        string enumName = value.GetType().Name;

        string valueAsString = string.Join(" | ", valueStringParts.Select(x => $"{enumName}.{x}"));

        return valueStringParts.Length > 1 && wrapCombinationalValueWithParentheses
            ? $"({valueAsString})"
            : valueAsString;
    }
}
