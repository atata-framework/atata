using System;
using System.Text.RegularExpressions;

namespace Atata
{
    /// <summary>
    /// Provides a set of methods for URI manipulations.
    /// </summary>
    public static class UriUtils
    {
        private static readonly Regex s_urlSchemaRegex = new("^[a-z]+://");

        /// <summary>
        /// Tries to create an absolute <see cref="Uri"/>.
        /// </summary>
        /// <param name="urlString">The URL string.</param>
        /// <param name="result">The result containing constructed <see cref="Uri"/>.</param>
        /// <returns>
        /// A <see cref="bool"/> value that is <see langword="true"/> if the <see cref="Uri"/> was successfully created;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryCreateAbsoluteUrl(string urlString, out Uri result)
        {
            if (urlString != null && s_urlSchemaRegex.IsMatch(urlString))
            {
                return Uri.TryCreate(urlString, UriKind.Absolute, out result);
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Concatenates the specified base URI with the relative URI.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The created <see cref="Uri"/>.</returns>
        public static Uri Concat(string baseUri, string relativeUri)
        {
            string fullUrl = baseUri ?? throw new ArgumentNullException(nameof(baseUri));

            if (!string.IsNullOrWhiteSpace(relativeUri))
            {
                if (baseUri.EndsWith("/", StringComparison.Ordinal) && relativeUri.StartsWith("/", StringComparison.Ordinal))
                    fullUrl += relativeUri.Substring(1);
                else if (!baseUri.EndsWith("/", StringComparison.Ordinal) && !relativeUri.StartsWith("/", StringComparison.Ordinal))
                    fullUrl += "/" + relativeUri;
                else
                    fullUrl += relativeUri;
            }

            return new Uri(fullUrl);
        }

        internal static string MergeAsString(string uri1, string uri2)
        {
            if (uri1 is null)
                return uri2;
            else if (uri2 is null)
                return uri1;
            else if (uri1.Length == 0)
                return uri2;
            else if (uri2.Length == 0)
                return uri1;

            return DecomposedUri.Merge(uri1, uri2);
        }
    }
}
