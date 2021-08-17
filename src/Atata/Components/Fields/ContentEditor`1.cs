namespace Atata
{
    /// <summary>
    /// Represents the content editor control (any element with <c>contenteditable='true'</c> or <c>contenteditable=''</c> attribute).
    /// This control is good to use for WYSIWYG editors.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[@contenteditable='' or @contenteditable='true']", ComponentTypeName = "content editor")]
    [GetsValueFromContent]
    public class ContentEditor<TOwner> : EditableTextField<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
