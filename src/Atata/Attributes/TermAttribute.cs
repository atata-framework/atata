﻿#nullable enable

namespace Atata;

/// <summary>
/// Specifies the term(s) to use for the control search.
/// </summary>
public class TermAttribute : TermSettingsAttribute
{
    public TermAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public TermAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public TermAttribute(TermMatch match, params string[] values)
        : base(match)
    {
        if (values?.Length > 0)
            Values = values;
    }

    public TermAttribute(params string[] values)
    {
        if (values?.Length > 0)
            Values = values;
    }

    /// <summary>
    /// Gets the term values.
    /// </summary>
    public string[]? Values
    {
        get => OptionalProperties.GetOrDefault<string[]>(nameof(Values));
        private set => OptionalProperties[nameof(Values)] = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the name should be cut
    /// considering the IgnoreNameEndings property value
    /// of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool CutEnding
    {
        get => OptionalProperties.GetOrDefault(nameof(CutEnding), true);
        set => OptionalProperties[nameof(CutEnding)] = value;
    }
}
