namespace Atata;

public class PressKeysLogSection : UIComponentLogSection
{
    public PressKeysLogSection(UIComponent component, string keys)
        : base(component)
    {
        Keys = keys;
        Message = $"Press \"{SpecialKeys.Replace(keys)}\" key{(keys?.Length == 1 ? null : "s")}";
    }

    public string Keys { get; }
}
