namespace Atata.KendoUI
{
    [PageObjectDefinition("div", ContainingClass = "k-window", ComponentTypeName = "window", IgnoreNameEndings = "PopupWindow,Window,Popup")]
    [WindowTitleElementDefinition(ContainingClass = "k-window-title")]
    public abstract class KendoWindow<TOwner> : PopupWindow<TOwner>
        where TOwner : KendoWindow<TOwner>
    {
        protected KendoWindow(params string[] windowTitleValues)
            : base(windowTitleValues)
        {
        }
    }
}
