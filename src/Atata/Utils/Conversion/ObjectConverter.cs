using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public class ObjectConverter : IObjectConverter
    {
        public string AssemblyNamePatternToFindTypes { get; set; } =
            @"^(?!System($|\..+)|mscorlib$|netstandard$|Microsoft\..+)";

        public object Convert(object sourceValue, Type destinationType)
        {
            destinationType.CheckNotNull(nameof(destinationType));

            if (sourceValue == null)
            {
                if (destinationType.IsClassOrNullable())
                    return null;
                else
                    throw ConversionException.For(null, destinationType);
            }

            Type underlyingDestinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
            Type sourceValueType = sourceValue.GetType();

            if (destinationType.IsAssignableFrom(sourceValueType) || underlyingDestinationType.IsAssignableFrom(sourceValueType))
                return sourceValue;
            if (destinationType.IsArray)
                return ConvertToArray(sourceValue, destinationType.GetElementType());
            if (TryConvertToEnumerable(sourceValue, destinationType, out object result))
                return result;
            if (underlyingDestinationType.IsEnum)
                return ConvertToEnum(destinationType, sourceValue);
            else if (underlyingDestinationType == typeof(TimeSpan))
                return ConvertToTimeSpan(sourceValue);
            else if (destinationType == typeof(Type))
                return ConvertToType(sourceValue);
            else
                return ConvertViaSystemConversion(sourceValue, underlyingDestinationType);
        }

        private static Array CreateArrayOfOneElement(Type elementType, object value)
        {
            Array array = Array.CreateInstance(elementType, 1);
            array.SetValue(value, 0);
            return array;
        }

        private static Array CreateArray(Type elementType, IEnumerable<object> enumerable)
        {
            Array array = Array.CreateInstance(elementType, enumerable.Count());

            int i = 0;

            foreach (var item in enumerable)
            {
                array.SetValue(item, i);
                i++;
            }

            return array;
        }

        private static object CreateEnumerable(Type type, object sourceEnumerable) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                ? CreateEnumerable(
                    typeof(ReadOnlyCollection<>).MakeGenericType(type.GetGenericArguments()[0]),
                    sourceEnumerable)
                : Activator.CreateInstance(type, sourceEnumerable);

        private static object ConvertToEnum(Type enumType, object value) =>
            value is string stringValue
                ? Enum.Parse(enumType, stringValue, true)
                : Enum.ToObject(enumType, value);

        private static TimeSpan ConvertToTimeSpan(object value) =>
            value is double or int or float
                ? TimeSpan.FromSeconds(System.Convert.ToDouble(value))
                : TimeSpan.Parse(value.ToString());

        private static bool TryGetIEnumerableElementType(Type type, out Type elementType)
        {
            var enumerableType = new[] { type }
                .Concat(type.GetInterfaces())
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            elementType = enumerableType?.GetGenericArguments()[0];
            return enumerableType != null;
        }

        private static object ConvertViaSystemConversion(object value, Type destinationType) =>
            System.Convert.ChangeType(value, destinationType);

        private object ConvertToArray(object value, Type elementType)
        {
            var originalValueType = value.GetType();

            if (originalValueType.IsArray && originalValueType.GetElementType() == elementType)
            {
                return value;
            }
            else if (originalValueType == elementType)
            {
                return CreateArrayOfOneElement(elementType, value);
            }
            else if (TryGetIEnumerableElementType(originalValueType, out Type originalValueElementType))
            {
                var valueAsEnumerable = ((IEnumerable)value).Cast<object>();

                if (originalValueElementType != elementType)
                    valueAsEnumerable = valueAsEnumerable.Select(x => Convert(x, elementType)).ToArray();

                return CreateArray(elementType, valueAsEnumerable);
            }
            else
            {
                var convertedValue = Convert(value, elementType);

                return CreateArrayOfOneElement(elementType, convertedValue);
            }
        }

        private bool TryConvertToEnumerable(object value, Type destinationType, out object result)
        {
            if (typeof(IEnumerable).IsAssignableFrom(destinationType)
                && TryGetIEnumerableElementType(destinationType, out Type destinationElementType)
                && TryGetIEnumerableElementType(value.GetType(), out Type originalValueElementType))
            {
                if (originalValueElementType != destinationElementType)
                {
                    var valueAsEnumerable = ((IEnumerable)value)
                        .Cast<object>()
                        .Select(x => Convert(x, destinationElementType));

                    Array valueAsArray = CreateArray(destinationElementType, valueAsEnumerable);
                    result = CreateEnumerable(destinationType, valueAsArray);
                }
                else
                {
                    result = CreateEnumerable(destinationType, value);
                }

                return true;
            }

            result = null;
            return false;
        }

        private Type ConvertToType(object value)
        {
            if (value is Type valueAsType)
            {
                return valueAsType;
            }
            else if (value is string typeName)
            {
                Assembly[] assemblies = AssemblyFinder.FindAllByPattern(AssemblyNamePatternToFindTypes);
                return TypeFinder.FindInAssemblies(typeName, assemblies);
            }
            else
            {
                throw ConversionException.For(value, typeof(Type));
            }
        }
    }
}
