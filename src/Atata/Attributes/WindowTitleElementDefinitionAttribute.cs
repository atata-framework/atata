namespace Atata
{
    /// <summary>
    /// Specifies the definition of the window title element which belongs to <see cref="PopupWindow{TOwner}"/>.
    /// It is used by <see cref="PopupWindow{TOwner}"/> together with <see cref="WindowTitleAttribute"/>.
    /// </summary>
    /// <seealso cref="PopupWindow{TOwner}"/>
    /// <seealso cref="WindowTitleAttribute"/>
    public class WindowTitleElementDefinitionAttribute : ScopeDefinitionAttribute
    {
        public WindowTitleElementDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
            : base(scopeXPath)
        {
        }
    }
}
