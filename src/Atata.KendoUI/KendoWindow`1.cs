namespace Atata.KendoUI
{
    [PageObjectDefinition("div", ContainingClass = "k-window", ComponentTypeName = "window", IgnoreNameEndings = "PopupWindow,Window,Popup")]
    [WindowTitleElementDefinition(ContainingClass = "k-window-title")]
    public abstract class KendoWindow<T> : PopupWindow<T>
        where T : KendoWindow<T>
    {
    }
}
