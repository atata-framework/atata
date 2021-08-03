using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Atata
{
    /// <summary>
    /// Provides a set of methods for assembly finding.
    /// </summary>
    public static class AssemblyFinder
    {
        private static readonly ConcurrentDictionary<string, Assembly[]> s_assembliesMatchingNamePattern =
            new ConcurrentDictionary<string, Assembly[]>();

        /// <summary>
        /// Finds the assembly by name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>The found assembly.</returns>
        /// <exception cref="AssemblyNotFoundException">Assembly not found.</exception>
        public static Assembly Find(string assemblyName)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            return allAssemblies.FirstOrDefault(x => x.GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
                ?? throw AssemblyNotFoundException.For(assemblyName);
        }

        /// <summary>
        /// Finds all assemblies that match the specified regex patterns.
        /// </summary>
        /// <param name="assemblyNamePatterns">The assembly name patterns.</param>
        /// <returns>The found assemblies.</returns>
        public static Assembly[] FindAllByPatterns(params string[] assemblyNamePatterns) =>
            FindAllByPatterns(assemblyNamePatterns.AsEnumerable());

        /// <summary>
        /// Finds all assemblies that match the specified regex patterns.
        /// </summary>
        /// <param name="assemblyNamePatterns">The assembly name patterns.</param>
        /// <returns>The found assemblies.</returns>
        public static Assembly[] FindAllByPatterns(IEnumerable<string> assemblyNamePatterns)
        {
            assemblyNamePatterns.CheckNotNullOrEmpty(nameof(assemblyNamePatterns));

            return assemblyNamePatterns.Any(x => string.IsNullOrEmpty(x))
                ? FindAllByPattern(string.Empty)
                : assemblyNamePatterns.SelectMany(
                    FindAllByPattern)
                    .Distinct()
                    .ToArray();
        }

        /// <summary>
        /// Finds all assemblies that match the specified regex pattern.
        /// </summary>
        /// <param name="assemblyNamePattern">The assembly name pattern.</param>
        /// <returns>The found assemblies.</returns>
        public static Assembly[] FindAllByPattern(string assemblyNamePattern)
        {
            return s_assembliesMatchingNamePattern.GetOrAdd(
                assemblyNamePattern ?? string.Empty,
                DoFindAllByPattern);
        }

        private static Assembly[] DoFindAllByPattern(string assemblyNamePattern)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (string.IsNullOrEmpty(assemblyNamePattern))
            {
                return allAssemblies;
            }
            else
            {
                Regex regex = new Regex(assemblyNamePattern);

                return allAssemblies
                    .Where(x => regex.IsMatch(x.GetName().Name))
                    .ToArray();
            }
        }
    }
}
