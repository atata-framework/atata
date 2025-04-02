#nullable enable

namespace Atata;

/// <summary>
/// Specifies the window title of <see cref="PopupWindow{TOwner}"/> page object.
/// This title can be used to identify the popup window among others when there can be several opened popups at the same time.
/// It is used by <see cref="PopupWindow{TOwner}"/> together with <see cref="WindowTitleElementDefinitionAttribute"/>.
/// </summary>
/// <seealso cref="PopupWindow{TOwner}"/>
/// <seealso cref="WindowTitleElementDefinitionAttribute"/>
public class WindowTitleAttribute : MulticastAttribute, ITermSettings
{
    private const TermCase DefaultCase = TermCase.Title;

    private const TermMatch DefaultMatch = TermMatch.Equals;

    public WindowTitleAttribute(TermCase termCase)
        : this(null, termCase: termCase)
    {
    }

    public WindowTitleAttribute(TermMatch match, TermCase termCase = DefaultCase)
        : this(null, match, termCase)
    {
    }

    public WindowTitleAttribute(TermMatch match, params string[] values)
        : this(values, match)
    {
    }

    public WindowTitleAttribute(params string[] values)
        : this(values, DefaultMatch)
    {
    }

    private WindowTitleAttribute(string[]? values, TermMatch match = DefaultMatch, TermCase termCase = DefaultCase)
    {
        Values = values;
        Match = match;
        Case = termCase;
    }

    public string[]? Values { get; }

    public TermCase Case { get; }

    public new TermMatch Match { get; }

    public string? Format { get; set; }

    internal string[] ResolveActualValues(string fallbackValue)
    {
        string[] rawValues = Values?.Length > 0
            ? Values
            : [Case.ApplyTo(fallbackValue ?? throw new ArgumentNullException(nameof(fallbackValue)))];

        return Format?.Length > 0
            ? [.. rawValues.Select(x => string.Format(Format, x))]
            : rawValues;
    }
}
