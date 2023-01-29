using System;

namespace Atata
{
    /// <summary>
    /// <para>
    /// Specifies the URL to navigate to during initialization of page object.
    /// Applies to page object types.
    /// The URL can be either absolute or relative.
    /// In case of relative URL, it is concatenated with the <see cref="AtataContext.BaseUrl"/>.
    /// </para>
    /// <para>
    /// The URL can be represented in a template format, like <c>"/organization/{OrganizationId}/details"</c>.
    /// The template is filled with <see cref="AtataContext.Variables"/>
    /// by using <see cref="AtataContext.FillUriTemplateString(string)"/> method.
    /// </para>
    /// </summary>
    public class UrlAttribute : MulticastAttribute
    {
        public UrlAttribute(string value) =>
            Value = value;

        [Obsolete("Use " + nameof(Value) + " instead.")] // Obsolete since v2.7.0.
        public string Url => Value;

        /// <summary>
        /// Gets the URL value.
        /// </summary>
        public string Value { get; }
    }
}
