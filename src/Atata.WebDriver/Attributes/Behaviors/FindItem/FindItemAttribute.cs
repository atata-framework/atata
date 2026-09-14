namespace Atata.WebDriver;

public abstract class FindItemAttribute : MulticastAttribute
{
    public abstract IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata);
}
