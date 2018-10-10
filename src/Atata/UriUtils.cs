using System;
using System.Text.RegularExpressions;

namespace Atata
{
    internal static class UriUtils
    {
        private static readonly Regex UrlSchemaRegex = new Regex("^[a-z]+://");

        internal static bool TryCreateAbsoluteUrl(string urlString, out Uri result)
        {
            if (UrlSchemaRegex.IsMatch(urlString))
            {
                return Uri.TryCreate(urlString, UriKind.Absolute, out result);
            }
            else
            {
                result = null;
                return false;
            }
        }

        internal static Uri Concat(string baseUrl, string relativeUri)
        {
            string fullUrl = baseUrl;

            if (!string.IsNullOrWhiteSpace(relativeUri))
            {
                if (baseUrl.EndsWith("/") && relativeUri.StartsWith("/"))
                    fullUrl += relativeUri.Substring(1);
                else if (!baseUrl.EndsWith("/") && !relativeUri.StartsWith("/"))
                    fullUrl += "/" + relativeUri;
                else
                    fullUrl += relativeUri;
            }

            return new Uri(fullUrl);
        }
    }
}
