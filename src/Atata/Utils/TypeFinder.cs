using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Atata
{
    /// <summary>
    /// Provides a set of methods for type finding.
    /// </summary>
    public static class TypeFinder
    {
        private const char NamespaceSeparator = '.';

        private const char SubTypeSeparator = '+';

        private const char GenericTypeSeparator = '`';

        private static readonly ConcurrentDictionary<string, Type> s_typesByName = new();

        /// <summary>
        /// Finds the type by name in current application domain.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="useCache">Uses cache if set to <see langword="true"/>.</param>
        /// <returns>The found type.</returns>
        /// <exception cref="TypeNotFoundException">Type not found.</exception>
        public static Type FindInCurrentAppDomain(string typeName, bool useCache = true) =>
            FindInAssemblies(typeName, AppDomain.CurrentDomain.GetAssemblies(), useCache);

        /// <summary>
        /// Finds the type by name in the specified assemblies.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="assembliesToFindIn">The assemblies to find in.</param>
        /// <param name="useCache">Uses cache if set to <see langword="true"/>.</param>
        /// <returns>The found type.</returns>
        /// <exception cref="TypeNotFoundException">Type not found.</exception>
        public static Type FindInAssemblies(string typeName, IEnumerable<Assembly> assembliesToFindIn, bool useCache = true)
        {
            typeName.CheckNotNullOrWhitespace(nameof(typeName));
            assembliesToFindIn.CheckNotNull(nameof(assembliesToFindIn));

            Type DoFind(string name)
            {
                Type type = null;

                if (name.Contains(NamespaceSeparator))
                {
                    type = Type.GetType(name, throwOnError: false, ignoreCase: true)
                        ?? assembliesToFindIn.Select(x => x.GetType(typeName, throwOnError: false, ignoreCase: true))
                        .FirstOrDefault(x => x != null);
                }

                return type
                    ?? FindAmongTypes(typeName, assembliesToFindIn.SelectMany(x => x.GetTypes()))
                    ?? throw TypeNotFoundException.For(typeName, assembliesToFindIn);
            }

            return useCache
                ? s_typesByName.GetOrAdd(typeName, DoFind)
                : DoFind(typeName);
        }

        private static Type FindAmongTypes(string typeName, IEnumerable<Type> typesToFindAmong)
        {
            string pureTypeName;
            string namespacePart = null;
            string[] declaringTypeNames = new string[0];

            if (typeName.Contains(NamespaceSeparator))
            {
                int separatorIndex = typeName.LastIndexOf(NamespaceSeparator);
                namespacePart = typeName.Substring(0, separatorIndex);
                pureTypeName = typeName.Substring(separatorIndex + 1);
            }
            else
            {
                pureTypeName = typeName;
            }

            if (pureTypeName.Contains(SubTypeSeparator))
            {
                string[] typeNames = pureTypeName.Split(SubTypeSeparator);
                pureTypeName = typeNames[typeNames.Length - 1];
                declaringTypeNames = typeNames.Take(typeNames.Length - 1).ToArray();
            }

            IEnumerable<Type> matchingTypes = (pureTypeName.Contains(GenericTypeSeparator)
                ? FilterByExactName(typesToFindAmong, pureTypeName)
                : FilterByNameConsideringGeneric(typesToFindAmong, pureTypeName))
                .ToArray();

            if (namespacePart != null)
            {
                matchingTypes = FilterByNamespacePart(matchingTypes, namespacePart);
            }

            if (declaringTypeNames.Any())
            {
                matchingTypes = FilterByDeclaringTypeNames(matchingTypes, declaringTypeNames.Reverse());
            }

            if (pureTypeName.Contains(GenericTypeSeparator))
            {
                return matchingTypes.FirstOrDefault();
            }
            else
            {
                matchingTypes = matchingTypes.ToArray();

                return FilterByExactName(matchingTypes, pureTypeName).FirstOrDefault()
                    ?? matchingTypes.FirstOrDefault(type => !matchingTypes.Any(otherType => otherType != type && type.IsInheritedFromOrIs(otherType)));
            }
        }

        private static IEnumerable<Type> FilterByExactName(IEnumerable<Type> types, string typeName) =>
            types.Where(x => x.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

        private static IEnumerable<Type> FilterByNameConsideringGeneric(IEnumerable<Type> types, string typeName)
        {
            Regex nameRegex = CreateRegexForNameConsideringGeneric(typeName);

            return types
                .Where(x => x.Name.StartsWith(typeName, StringComparison.OrdinalIgnoreCase))
                .Where(x => nameRegex.IsMatch(x.Name));
        }

        private static IEnumerable<Type> FilterByDeclaringTypeNames(IEnumerable<Type> types, IEnumerable<string> declaringTypeNames)
        {
            bool DoesMatch(Type type)
            {
                Type currentType = type.DeclaringType;

                foreach (string typeName in declaringTypeNames)
                {
                    bool match = currentType is null
                        ? false
                        : typeName.Contains(GenericTypeSeparator)
                        ? currentType.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                        : CreateRegexForNameConsideringGeneric(typeName).IsMatch(currentType.Name);

                    if (!match)
                        return false;

                    currentType = currentType.DeclaringType;
                }

                return true;
            }

            return types.Where(DoesMatch);
        }

        private static IEnumerable<Type> FilterByNamespacePart(IEnumerable<Type> types, string namespacePart)
        {
            Regex regex = new Regex($@"(^|.+\.){namespacePart}$", RegexOptions.IgnoreCase);

            return types.Where(x => regex.IsMatch(x.Namespace));
        }

        private static Regex CreateRegexForNameConsideringGeneric(string typeName) =>
            new($"^{typeName}($|`.+)", RegexOptions.IgnoreCase);
    }
}
