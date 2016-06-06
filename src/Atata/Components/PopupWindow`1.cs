namespace Atata
{
    [PageObjectDefinition(ComponentTypeName = "window", IgnoreNameEndings = "Window,PopupWindow")]
    public class PopupWindow<T> : PageObject<T>
        where T : PopupWindow<T>
    {
        public PopupWindow(string windowTitle = null)
        {
            WindowTitle = windowTitle;
        }

        protected string WindowTitle { get; set; }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            WindowTitleAttribute titleAttribute = metadata.GetFirstOrDefaultAttribute<WindowTitleAttribute>();
            if (titleAttribute != null)
            {
                if (titleAttribute.Value != null)
                    WindowTitle = titleAttribute.Value;
                else if (titleAttribute.UseComponentName)
                    WindowTitle = ComponentName;
            }
        }
    }
}
