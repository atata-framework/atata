namespace Atata
{
    [PageObjectDefinition(ComponentTypeName = "window", IgnoreNameEndings = "PopupWindow,Window,Popup")]
    public abstract class PopupWindow<T> : PageObject<T>
        where T : PopupWindow<T>
    {
        protected PopupWindow(params string[] windowTitleValues)
        {
            WindowTitleValues = windowTitleValues;
        }

        protected string[] WindowTitleValues { get; set; }
        protected TermMatch WindowTitleMatch { get; set; } = TermMatch.Equals;

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            WindowTitleAttribute titleAttribute = metadata.GetFirstOrDefaultAttribute<WindowTitleAttribute>();
            if (titleAttribute != null)
            {
                WindowTitleValues = titleAttribute.GetActualValues(ComponentName);
                WindowTitleMatch = titleAttribute.Match;
            }
        }
    }
}
