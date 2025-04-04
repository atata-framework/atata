namespace Atata;

/// <summary>
/// <para>
/// Specifies the URL to navigate to during initialization of page object.
/// Applies to page object types.
/// The URL can be either absolute or relative.
/// In case of relative URL, it is concatenated with the <see cref="AtataContext.BaseUrl"/>.
/// </para>
/// <para>
/// The URL can be represented in a template format, like <c>"/organization/{OrganizationId}/details"</c>.
/// The template is filled with <see cref="AtataSession.Variables"/>
/// by using <see cref="VariableHierarchicalDictionary.FillUriTemplateString(string)"/> method.
/// </para>
/// </summary>
public class UrlAttribute : MulticastAttribute
{
    public UrlAttribute(string? value) =>
        Value = value;

    /// <summary>
    /// Gets the URL value.
    /// </summary>
    public string? Value { get; }
}
