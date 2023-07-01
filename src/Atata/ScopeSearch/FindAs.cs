namespace Atata;

/// <summary>
/// Specifies the way <see cref="FindAttribute"/> should be used.
/// </summary>
public enum FindAs
{
    /// <summary>
    /// Use the attribute as a scope element locator.
    /// </summary>
    Scope,

    /// <summary>
    /// Use the attribute as a parent layer.
    /// It means that the scope element or next layer element is located as a child of this one in DOM.
    /// </summary>
    Parent,

    /// <summary>
    /// Use the attribute as an ancestor layer.
    /// It means that the scope element or next layer element is located as a descendant of this one in DOM.
    /// </summary>
    Ancestor,

    /// <summary>
    /// Use the attribute as a shadow host layer.
    /// It means that the scope element or next layer element is located inside the shadow root of this one in DOM.
    /// </summary>
    ShadowHost,

    /// <summary>
    /// Use the attribute as a sibling layer.
    /// It means that the scope element or next layer element is located as a sibling of this one in DOM.
    /// </summary>
    Sibling
}
